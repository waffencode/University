using University.Domain;
using University.Domain.Model;
using University.Exceptions;
using University.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using University.Repository;
using University.Service;
using University.Test.TestData;

namespace University.Test;

public class ScheduleClassServiceTest
{
    private readonly DbContextOptions<UniversityContext> _dbContextOptions;
    private readonly Mock<ILogger<ScheduleClassService>> _mockLogger;

    public ScheduleClassServiceTest()
    {
        _dbContextOptions = new DbContextOptionsBuilder<UniversityContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _mockLogger = new Mock<ILogger<ScheduleClassService>>();
    }

    [Fact]
    public async Task CreateScheduleClassAsync_ShouldCreateWithStudentDetails()
    {
        // Arrange
        var teacherId = Guid.NewGuid();
        var timeSlotId = Guid.NewGuid();
        var classroomId = Guid.NewGuid();
        var subjectWorkProgramId = Guid.NewGuid();
        var studyGroupId = Guid.NewGuid();
        var studentId = Guid.NewGuid();

        await using (var arrangeContext = new UniversityContext(_dbContextOptions))
        {
            await arrangeContext.Database.EnsureCreatedAsync();

            var teacher = new User { Id = teacherId, Email = "teacher@example.com" };
            var timeSlot = new ClassTimeSlot { Id = timeSlotId, Name = "Slot 1", StartTime = TimeOnly.MinValue, EndTime = TimeOnly.MaxValue };
            var classroom = new Classroom { Id = classroomId, Designation = "Room 101" };
            var subjectWorkProgram = new SubjectWorkProgram 
            { 
                Id = subjectWorkProgramId, 
                Subject = new Subject { Name = "Math" } 
            };
            var student = new User { Id = studentId, Email = "student@example.com" };
            var studyGroup = new StudyGroup
            {
                Id = studyGroupId,
                Students = new List<User> { student },
                FieldOfStudy = new FieldOfStudy { Code = "CS101" }
            };

            await arrangeContext.Users.AddRangeAsync(teacher, student);
            await arrangeContext.ClassTimeSlots.AddAsync(timeSlot);
            await arrangeContext.Classrooms.AddAsync(classroom);
            await arrangeContext.SubjectWorkPrograms.AddAsync(subjectWorkProgram);
            await arrangeContext.StudyGroups.AddAsync(studyGroup);
            await arrangeContext.SaveChangesAsync();
        }

        var dto = new ScheduleClassDto
        {
            Id = Guid.NewGuid(),
            TeacherId = teacherId,
            TimeSlotId = timeSlotId,
            ClassroomId = classroomId,
            SubjectWorkProgramId = subjectWorkProgramId,
            GroupsId = [studyGroupId]
        };

        await using var actContext = new UniversityContext(_dbContextOptions);
        var repository = new ScheduleClassRepository(actContext, Mock.Of<ILogger<ScheduleClassRepository>>());
        var userRepository = new UserRepository(actContext);
        var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

        // Act
        await service.CreateScheduleClassAsync(dto, CancellationToken.None);

        // Assert
        await using (new UniversityContext(_dbContextOptions))
        {
            var createdClass = await repository.GetAsEntityByIdAsync(dto.Id, CancellationToken.None);

            Assert.NotNull(createdClass);
            Assert.NotNull(createdClass.Details);
            
            var studentDetails = createdClass.Details.StudentDetailsList.ToList();
            Assert.Single(studentDetails);
            Assert.Equal(studentId, studentDetails[0].Student.Id);
            Assert.Equal(AttendanceType.Absent, studentDetails[0].Attendance);
        }
    }

    [Fact]
    public async Task CreateScheduleClassAsync_WhenDependencyMissing_ThrowsEntityNotFound()
    {
        // Arrange
        var invalidDto = new ScheduleClassDto
        {
            TeacherId = Guid.NewGuid(), // Несуществующий
            TimeSlotId = Guid.NewGuid(),
            ClassroomId = Guid.NewGuid(),
            SubjectWorkProgramId = Guid.NewGuid(),
            GroupsId = []
        };

        await using var context = new UniversityContext(_dbContextOptions);
        var repository = new ScheduleClassRepository(context, Mock.Of<ILogger<ScheduleClassRepository>>());
        var userRepository = new UserRepository(context);
        var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => 
            service.CreateScheduleClassAsync(invalidDto, CancellationToken.None));
    }

    [Fact]
    public async Task CreateScheduleClassAsync_WhenEntityMissingAfterAdd_ThrowsInvalidOperation()
    {
        // Arrange
        var dto = new ScheduleClassDto { Id = Guid.NewGuid() };

        await using var actContext = new UniversityContext(_dbContextOptions);
        var repository = new ScheduleClassRepository(actContext, Mock.Of<ILogger<ScheduleClassRepository>>());
        var userRepository = new UserRepository(actContext);
        var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(() => 
            service.CreateScheduleClassAsync(dto, CancellationToken.None));
    }
    
    [Fact]
public async Task UpdateScheduleClassJournalAsync_ShouldUpdateStudentDetails()
{
    // Arrange
    var teacher = ScheduleClassTestData.CreateTestTeacher();
    var student1 = ScheduleClassTestData.CreateTestStudent();
    var student2 = ScheduleClassTestData.CreateTestStudent();
    var studyGroup = ScheduleClassTestData.CreateTestStudyGroup(new List<User> { student1 });
    
    var scheduleClass = ScheduleClassTestData.CreateValidScheduleClass(
        teacher: teacher,
        groups: new List<StudyGroup> { studyGroup });

    await using (var arrangeContext = new UniversityContext(_dbContextOptions))
    {
        await arrangeContext.Database.EnsureCreatedAsync();
        await arrangeContext.Users.AddRangeAsync(teacher, student1, student2);
        await arrangeContext.StudyGroups.AddAsync(studyGroup);
        await arrangeContext.ScheduleClasses.AddAsync(scheduleClass);
        await arrangeContext.SaveChangesAsync();
    }

    var updatedDetails = new ScheduleClassDetailsDto
    {
        StudentDetailsDtoList = new List<StudentDetailsDto>
        {
            new() // Обновляем существующего студента
            {
                StudentId = student1.Id,
                Attendance = AttendanceType.Present,
                Grade = 5
            },
            new() // Добавляем нового студента
            {
                StudentId = student2.Id,
                Attendance = AttendanceType.Sick,
                Grade = 4
            }
        }
    };

    await using var actContext = new UniversityContext(_dbContextOptions);
    var repository = new ScheduleClassRepository(actContext, Mock.Of<ILogger<ScheduleClassRepository>>());
    var userRepository = new UserRepository(actContext);
    var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

    // Act
    await service.UpdateScheduleClassJournalAsync(scheduleClass.Id, updatedDetails, CancellationToken.None);

    // Assert
    await using (var assertContext = new UniversityContext(_dbContextOptions))
    {
        var assertRepository = new ScheduleClassRepository(assertContext, Mock.Of<ILogger<ScheduleClassRepository>>());
        var updatedClass = await assertRepository.GetAsEntityByIdAsync(scheduleClass.Id, CancellationToken.None);

        Assert.NotNull(updatedClass);
        Assert.NotNull(updatedClass.Details);
        
        var studentDetails = updatedClass.Details.StudentDetailsList.ToList();
        Assert.Equal(2, studentDetails.Count);

        var originalStudentDetail = studentDetails.FirstOrDefault(sd => sd.Student.Id == student1.Id);
        Assert.NotNull(originalStudentDetail);
        Assert.Equal(AttendanceType.Present, originalStudentDetail.Attendance);
        Assert.Equal(5, originalStudentDetail.Grade);

        var newStudentDetail = studentDetails.FirstOrDefault(sd => sd.Student.Id == student2.Id);
        Assert.NotNull(newStudentDetail);
        Assert.Equal(AttendanceType.Sick, newStudentDetail.Attendance);
        Assert.Equal(4, newStudentDetail.Grade);
    }
}

[Fact]
public async Task UpdateScheduleClassJournalAsync_WhenClassNotFound_ThrowsInvalidOperation()
{
    // Arrange
    var nonExistentClassId = Guid.NewGuid();
    var detailsDto = new ScheduleClassDetailsDto { StudentDetailsDtoList = new List<StudentDetailsDto>() };

    await using var context = new UniversityContext(_dbContextOptions);
    var repository = new ScheduleClassRepository(context, Mock.Of<ILogger<ScheduleClassRepository>>());
    var userRepository = new UserRepository(context);
    var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

    // Act & Assert
    await Assert.ThrowsAsync<InvalidOperationException>(() => 
        service.UpdateScheduleClassJournalAsync(nonExistentClassId, detailsDto, CancellationToken.None));
}

[Fact]
public async Task UpdateScheduleClassJournalAsync_ShouldClearExistingDetailsWhenEmptyListProvided()
{
    // Arrange
    var teacher = ScheduleClassTestData.CreateTestTeacher();
    var student = ScheduleClassTestData.CreateTestStudent();
    var studyGroup = ScheduleClassTestData.CreateTestStudyGroup(new List<User> { student });
    
    var scheduleClass = ScheduleClassTestData.CreateValidScheduleClass(
        teacher: teacher,
        groups: new List<StudyGroup> { studyGroup });

    await using (var arrangeContext = new UniversityContext(_dbContextOptions))
    {
        await arrangeContext.Database.EnsureCreatedAsync();
        await arrangeContext.Users.AddRangeAsync(teacher, student);
        await arrangeContext.StudyGroups.AddAsync(studyGroup);
        await arrangeContext.ScheduleClasses.AddAsync(scheduleClass);
        await arrangeContext.SaveChangesAsync();
    }

    var emptyDetails = new ScheduleClassDetailsDto
    {
        StudentDetailsDtoList = new List<StudentDetailsDto>()
    };

    await using var actContext = new UniversityContext(_dbContextOptions);
    var repository = new ScheduleClassRepository(actContext, Mock.Of<ILogger<ScheduleClassRepository>>());
    var userRepository = new UserRepository(actContext);
    var service = new ScheduleClassService(repository, userRepository, _mockLogger.Object);

    // Act
    await service.UpdateScheduleClassJournalAsync(scheduleClass.Id, emptyDetails, CancellationToken.None);

    // Assert
    await using (var assertContext = new UniversityContext(_dbContextOptions))
    {
        var assertRepository = new ScheduleClassRepository(assertContext, Mock.Of<ILogger<ScheduleClassRepository>>());
        var updatedClass = await assertRepository.GetAsEntityByIdAsync(scheduleClass.Id, CancellationToken.None);

        Assert.NotNull(updatedClass);
        Assert.NotNull(updatedClass.Details);
        Assert.Empty(updatedClass.Details.StudentDetailsList);
    }
}
}