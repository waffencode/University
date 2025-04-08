using University.Domain;
using University.Infrastructure;
using University.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace University.Test;

public class SubjectWorkProgramRepositoryTest
{
    private readonly DbContextOptions<UserContext> _dbContextOptions;

    public SubjectWorkProgramRepositoryTest()
    {
        // Создаем InMemory базу данных для тестов
        _dbContextOptions = new DbContextOptionsBuilder<UserContext>()
            .UseInMemoryDatabase(databaseName: "UniversityTestDb")
            .Options;
    }

    [Fact]
    public async Task AddAsync_WhenSubjectExists_ShouldAddWorkProgramWithClasses()
    {
        Guid id = Guid.NewGuid();
        
        // Arrange
        using (var context = new UserContext(_dbContextOptions))
        {
            // Очищаем базу данных перед тестом
            context.Database.EnsureDeleted();
            
            // Создаем и добавляем тестовый Subject
            var subject = new Subject { Id = id, Name = "Test Subject" };
            context.Subjects.Add(subject);
            await context.SaveChangesAsync();
        }

        SubjectWorkProgram workProgram;
        using (var context = new UserContext(_dbContextOptions))
        {
            var repository = new SubjectWorkProgramRepository(context);
            
            var newSubject = new Subject { Id = id, Name = "Test Subject" };
            
            // Создаем тестовую рабочую программу с классом
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
        using (var context = new UserContext(_dbContextOptions))
        {
            // Проверяем, что рабочая программа добавилась
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