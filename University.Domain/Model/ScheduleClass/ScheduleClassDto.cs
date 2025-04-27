namespace University.Domain;

public class ScheduleClassDto
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public Guid TeacherId { get; set; }

    public DateOnly Date { get; set; }
    
    public Guid TimeSlotId { get; set; }
    
    public Guid ClassroomId { get; set; }
    
    public Guid SubjectWorkProgramId { get; set; }

    public ClassType ClassType { get; set; } = ClassType.Lecture;

    public IEnumerable<Guid> GroupsId { get; set; } = [];
    
    public Guid DetailsId { get; set; }
}