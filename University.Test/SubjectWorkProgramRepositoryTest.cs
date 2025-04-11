using University.Domain;
using University.Infrastructure;
using University.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace University.Test;

public class SubjectWorkProgramRepositoryTest
{
    private readonly DbContextOptions<UniversityContext> _dbContextOptions;

    public SubjectWorkProgramRepositoryTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<UniversityContext>()
            .UseInMemoryDatabase(databaseName: "UniversityTestDb")
            .Options;
    }

    [Fact]
    public async Task AddAsync_WhenSubjectExists_ShouldAddWorkProgramWithClasses()
    {
        Guid id = Guid.NewGuid();
        
        // Arrange
        using (var context = new UniversityContext(_dbContextOptions))
        {
            context.Database.EnsureDeleted();

            var subject = new Subject { Id = id, Name = "Test Subject" };
            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
        }

        SubjectWorkProgram workProgram;
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var repository = new SubjectWorkProgramRepository(context);
            
            var newSubject = new Subject { Id = id, Name = "Test Subject" };
            
            workProgram = new SubjectWorkProgram 
            { 
                Subject = newSubject,
                Classes = new List<PlannedClass> 
                { 
                    new PlannedClass 
                    { 
                        Theme = "Test Theme", 
                        Hours = 2, 
                        ClassType = ClassType.Lecture 
                    } 
                }
            };

            // Act
            await repository.AddAsync(workProgram);
        }

        // Assert
        using (var context = new UniversityContext(_dbContextOptions))
        {
            var savedWorkProgram = await context.SubjectWorkPrograms
                .Include(wp => wp.Subject)
                .Include(wp => wp.Classes)
                .FirstOrDefaultAsync();
            
            Assert.NotNull(savedWorkProgram);
            Assert.Equal(workProgram.Subject.Id, savedWorkProgram.Subject.Id);
            Assert.Single(savedWorkProgram.Classes);
            
            var savedClass = savedWorkProgram.Classes.First();
            Assert.Equal("Test Theme", savedClass.Theme);
            Assert.Equal(2, savedClass.Hours);
            Assert.Equal(ClassType.Lecture, savedClass.ClassType);
        }
    }
}