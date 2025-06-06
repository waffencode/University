namespace University.Domain;

public class Message
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Topic { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.Now;
    
    public bool IsImportant { get; set; }
    
    public required User Sender { get; set; }

    public List<User> Receivers { get; set; } = [];

    public List<StudyGroup> ReceiversStudyGroups { get; set; } = [];
    
    public List<Uri> Attachments { get; set; } = [];

    public ScheduleClass? RelatedClass { get; set; }
}