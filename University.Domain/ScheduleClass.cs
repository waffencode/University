namespace University.Domain;

public enum ClassType
{
    Unknown,
    Lecture,
    Lab,
    Seminar,
    Exam
}

public class ScheduleClass
{
    public Guid Id { get; set; } 

    public string Name { get; set; } = string.Empty;

    public required User Professor { get; set; }

    public DateOnly Date { get; set; }
    
    public required ClassTimeSlot TimeSlot { get; set; }

    public ClassType ClassType { get; set; } = ClassType.Unknown;
}