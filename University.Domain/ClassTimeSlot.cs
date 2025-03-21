namespace University.Domain;

public class ClassTimeSlot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public required TimeOnly StartTime { get; set; }
    public required TimeOnly EndTime { get; set; }
}