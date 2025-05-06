using System.ComponentModel.DataAnnotations;

namespace University.Domain.Model;

public enum AttendanceType
{
    Present,
    Sick,
    Excused,
    Absent
}

public class StudentDetails
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public User Student { get; set; }
    
    public AttendanceType Attendance { get; set; }

    [Range(1, 5)]
    public int? Grade { get; set; }
}

public class ScheduleClassDetails
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public IEnumerable<StudentDetails> StudentDetailsList { get; set; } = [];
}