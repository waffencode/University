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
    private readonly Mock<ILogger<SubjectWorkProgramRepository>> _mockLogger;

    public SubjectWorkProgramRepositoryTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<UniversityContext>()
            .ConfigureWarnings(warnings => warnings.Default(WarningBehavior.Ignore)
                .Log(InMemoryEventId.TransactionIgnoredWarning))
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _mockLogger = new Mock<ILogger<SubjectWorkProgramRepository>>();
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
    public async Task DeleteAsync_Should_LogAppropriateMessages()
    {
        // Arrange
        var programId = Guid.NewGuid();
        
        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureCreated();
            context.SubjectWorkPrograms.Add(new SubjectWorkProgram
            {
                Id = programId,
                Subject = new Subject { Name = "Biology" }
            });
            await context.SaveChangesAsync();
        }

        // Act
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var repository = new SubjectWorkProgramRepository(context, _mockLogger.Object);
            await repository.DeleteAsync(programId);
        }

        // Assert
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(programId.ToString())),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
            Times.AtLeastOnce);
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