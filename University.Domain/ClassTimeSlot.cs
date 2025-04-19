using System.ComponentModel.DataAnnotations;

namespace University.Domain;

public class ClassTimeSlot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Name { get; set; } = string.Empty;
    
    [Range(1, 7, ErrorMessage = "Incorrect class ordinal (it must be between 1 and 7).")]
    public int Ordinal { get; set; }
    
    public required TimeOnly StartTime { get; set; }
    
    public required TimeOnly EndTime { get; set; }
}