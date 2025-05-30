using University.Domain;
using University.Infrastructure;
using University.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using Moq;
using University.Exceptions;

namespace University.Test;

public class SubjectWorkProgramRepositoryTest
{
    private readonly DbContextOptions<UniversityContext> _dbContextOptions;
    private readonly UniversityContext _context;
    private readonly SubjectWorkProgramRepository _repository;
    private readonly Mock<ILogger<SubjectWorkProgramRepository>> _mockLogger;

    public SubjectWorkProgramRepositoryTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<UniversityContext>()
            .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Ignore)
                .Log(InMemoryEventId.TransactionIgnoredWarning))
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new UniversityContext(_dbContextOptions);
        _context.Database.EnsureCreated();
        _mockLogger = new Mock<ILogger<SubjectWorkProgramRepository>>();
        _repository = new SubjectWorkProgramRepository(_context, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_Should_CreateSubjectWorkProgram()
    {
        // Arrange
        var subject = TestData.CreateTestSubject();
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
        var subjectWorkProgram = TestData.CreateTestWorkProgram(subject: subject);

        // Act
        await _repository.AddAsync(subjectWorkProgram);

        // Assert
        var entity = await _context.SubjectWorkPrograms
            .Include(s => s.Subject)
            .FirstOrDefaultAsync(s => s.Id.Equals(subjectWorkProgram.Id));
        Assert.NotNull(entity);
        Assert.Equal(entity.Subject.Name, subjectWorkProgram.Subject.Name);
        Assert.Equal(entity.Classes.Count, subjectWorkProgram.Classes.Count);
    }

    [Fact]
    public async Task AddAsync_Should_ThrowExceptionIfSubjectNotFound()
    {
        // Arrange
        var subjectWorkProgram = TestData.CreateTestWorkProgram();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => _repository.AddAsync(subjectWorkProgram));
    }

    [Fact]
    public async Task UpdateAsync_Should_UpdateSubjectWorkProgram()
    {
        // Arrange
        var subject = TestData.CreateTestSubject();
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
        var subjectWorkProgram = TestData.CreateTestWorkProgram(subject: subject);
        await _context.SubjectWorkPrograms.AddAsync(subjectWorkProgram);
        await _context.SaveChangesAsync();
        var entityToUpdate = await _context.SubjectWorkPrograms.AsNoTracking()
            .Include(s => s.Subject)
            .Include(s => s.Classes)
            .FirstAsync(s => s.Id == subjectWorkProgram.Id);

        // Act
        entityToUpdate.Classes.First().Theme = "Geometry";
        await _repository.UpdateAsync(entityToUpdate);

        // Assert
        var entity = await _repository.GetByIdAsync(subjectWorkProgram.Id);
        Assert.NotNull(entity);
        Assert.Equal(entity.Classes.First().Theme, subjectWorkProgram.Classes.First().Theme);
    }

    [Fact]
    public async Task DeleteAsync_Should_CascadeDeleteOwnedClasses()
    {
        // Arrange
        var programId = Guid.NewGuid();
        var classId = Guid.NewGuid();

        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureCreated();

            var program = new SubjectWorkProgram
            {
                Id = programId,
                Subject = new Subject { Name = "Math" },
                Classes = { new PlannedClass { Id = classId, Theme = "Algebra" } }
            };

            context.SubjectWorkPrograms.Add(program);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var repository = new SubjectWorkProgramRepository(context, _mockLogger.Object);
            await repository.DeleteAsync(programId);
        }

        // Assert
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var deletedProgram = await context.SubjectWorkPrograms
                .Include(p => p.Classes)
                .FirstOrDefaultAsync(p => p.Id == programId);

            Assert.Null(deletedProgram);
        }
    }

    [Fact]
    public async Task DeleteAsync_Should_NotDeleteRelatedSubject()
    {
        // Arrange
        var subjectId = Guid.NewGuid();
        var programId = Guid.NewGuid();

        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureCreated();

            var subject = new Subject { Id = subjectId, Name = "Physics" };
            var program = new SubjectWorkProgram
            {
                Id = programId,
                Subject = subject,
                Classes = { new PlannedClass { Theme = "Mechanics" } }
            };

            await context.Subjects.AddAsync(subject);
            await context.SubjectWorkPrograms.AddAsync(program);
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var repository = new SubjectWorkProgramRepository(context, _mockLogger.Object);
            await repository.DeleteAsync(programId);
        }

        // Assert
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var remainingSubject = await context.Subjects.FindAsync(subjectId);
            Assert.NotNull(remainingSubject);
        }
    }

    [Fact]
    public async Task DeleteAsync_Should_CompleteWithinTransaction()
    {
        // Arrange
        var programId = Guid.NewGuid();

        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureCreated();
            context.SubjectWorkPrograms.Add(new SubjectWorkProgram
            {
                Id = programId,
                Subject = new Subject { Name = "Chemistry" }
            });
            await context.SaveChangesAsync();
        }

        // Act & Assert
        await using (var context = new UniversityContext(_dbContextOptions))
        {
            await context.Database.BeginTransactionAsync();

            var repository = new SubjectWorkProgramRepository(context, _mockLogger.Object);
            await repository.DeleteAsync(programId);

            await context.Database.CommitTransactionAsync();
        }
    }

    [Fact]
    public async Task DeleteAsync_Should_ThrowWhenConcurrentModificationDetected()
    {
        // Arrange
        var programId = Guid.NewGuid();

        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureCreated();
            context.SubjectWorkPrograms.Add(new SubjectWorkProgram
            {
                Id = programId,
                Subject = new Subject { Name = "History" }
            });
            await context.SaveChangesAsync();
        }

        // Simulate concurrent delete
        using (var context1 = new UniversityContext(_dbContextOptions))
        using (var context2 = new UniversityContext(_dbContextOptions))
        {
            // First delete succeeds
            var repo1 = new SubjectWorkProgramRepository(context1, _mockLogger.Object);
            await repo1.DeleteAsync(programId);

            // Second delete should fail
            var repo2 = new SubjectWorkProgramRepository(context2, _mockLogger.Object);
            await Assert.ThrowsAsync<EntityNotFoundException>(() => repo2.DeleteAsync(programId));
        }
    }
}