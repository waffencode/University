namespace University.Domain.Model;

public class StudentDetailsDto
{
    public Guid Id { get; set; }
    
    public Guid StudentId { get; set; }
    
    public AttendanceType Attendance { get; set; }
    
    public int? Grade { get; set; }
}

public class ScheduleClassDetailsDto
{
    public Guid Id { get; set; }

    public IEnumerable<StudentDetailsDto> StudentDetailsDtoList { get; set; } = [];
}