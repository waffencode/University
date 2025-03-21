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
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public required User Teacher { get; set; }

    public DateOnly Date { get; set; }
    
    public required ClassTimeSlot TimeSlot { get; set; }
    
    public required Classroom Classroom { get; set; }
    
    public required Subject Subject { get; set; }

    public ClassType ClassType { get; set; } = ClassType.Unknown;

    public required List<StudyGroup> Groups { get; set; } = [];
}