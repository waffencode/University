using University.Domain;
using University.Domain.Model;

namespace University.Test.TestData;

public static class ScheduleClassTestData
{
    public static ScheduleClass CreateValidScheduleClass(
        Guid? id = null,
        User teacher = null,
        ClassTimeSlot timeSlot = null,
        Classroom classroom = null,
        SubjectWorkProgram workProgram = null,
        List<StudyGroup> groups = null,
        List<User> students = null)
    {
        var scheduleClassId = id ?? Guid.NewGuid();
        var testTeacher = teacher ?? CreateTestTeacher();
        var testTimeSlot = timeSlot ?? CreateTestTimeSlot();
        var testClassroom = classroom ?? CreateTestClassroom();
        var testWorkProgram = workProgram ?? CreateTestWorkProgram();
        var testGroups = groups ?? new List<StudyGroup> { CreateTestStudyGroup(students) };

        return new ScheduleClass
        {
            Id = scheduleClassId,
            Name = "Test Schedule Class",
            Teacher = testTeacher,
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlot = testTimeSlot,
            Classroom = testClassroom,
            SubjectWorkProgram = testWorkProgram,
            ClassType = ClassType.Lecture,
            Groups = testGroups,
            Details = new ScheduleClassDetails
            {
                StudentDetailsList = testGroups
                    .SelectMany(g => g.Students)
                    .Select(s => new StudentDetails
                    {
                        Student = s,
                        Attendance = AttendanceType.Absent,
                        Grade = null
                    })
                    .ToList()
            }
        };
    }

    public static ScheduleClassDto CreateValidScheduleClassDto(
        Guid? id = null,
        Guid? teacherId = null,
        Guid? timeSlotId = null,
        Guid? classroomId = null,
        Guid? workProgramId = null,
        IEnumerable<Guid> groupIds = null)
    {
        return new ScheduleClassDto
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Test Schedule Class DTO",
            TeacherId = teacherId ?? Guid.NewGuid(),
            Date = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            TimeSlotId = timeSlotId ?? Guid.NewGuid(),
            ClassroomId = classroomId ?? Guid.NewGuid(),
            SubjectWorkProgramId = workProgramId ?? Guid.NewGuid(),
            ClassType = ClassType.Lecture,
            GroupsId = groupIds ?? new[] { Guid.NewGuid() },
            DetailsId = Guid.NewGuid()
        };
    }

    public static User CreateTestTeacher(Guid? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = "teacher@university.com",
            Role = UserRole.Teacher,
            FullName = "John Doe"
        };
    }

    public static ClassTimeSlot CreateTestTimeSlot(Guid? id = null)
    {
        return new ClassTimeSlot
        {
            Id = id ?? Guid.NewGuid(),
            Name = "Morning Slot",
            Ordinal = 1,
            StartTime = new TimeOnly(9, 0),
            EndTime = new TimeOnly(10, 30)
        };
    }

    public static Classroom CreateTestClassroom(Guid? id = null)
    {
        return new Classroom
        {
            Id = id ?? Guid.NewGuid(),
            Designation = "A-101",
            Name = "Lecture Hall",
            Type = ClassroomType.LectureHall
        };
    }

    public static SubjectWorkProgram CreateTestWorkProgram(Guid? id = null)
    {
        return new SubjectWorkProgram
        {
            Id = id ?? Guid.NewGuid(),
            Subject = new Subject { Name = "Mathematics" },
            Classes = new List<PlannedClass>
            {
                new() { Theme = "Algebra", Hours = 2, ClassType = ClassType.Lecture }
            }
        };
    }

    public static StudyGroup CreateTestStudyGroup(
        List<User> students = null,
        Guid? id = null,
        FieldOfStudy fieldOfStudy = null)
    {
        var testStudents = students ?? new List<User> { CreateTestStudent() };
        var testFieldOfStudy = fieldOfStudy ?? new FieldOfStudy
        {
            Code = "CS-101",
            Name = "Computer Science"
        };

        return new StudyGroup
        {
            Id = id ?? Guid.NewGuid(),
            Name = "CS-101 Group A",
            Students = testStudents,
            FieldOfStudy = testFieldOfStudy
        };
    }

    public static User CreateTestStudent(Guid? id = null)
    {
        return new User
        {
            Id = id ?? Guid.NewGuid(),
            Email = "student@university.com",
            Role = UserRole.Student,
            FullName = "Alice Smith"
        };
    }

    public static List<StudentDetails> CreateStudentDetailsList(int count = 3)
    {
        return Enumerable.Range(1, count)
            .Select(i => new StudentDetails
            {
                Student = CreateTestStudent(),
                Attendance = AttendanceType.Absent,
                Grade = null
            })
            .ToList();
    }
}