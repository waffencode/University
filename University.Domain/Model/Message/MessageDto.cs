namespace University.Domain;

public class MessageDto
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string Topic { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public DateTime Date { get; set; } = DateTime.Now;
    
    public bool IsImportant { get; set; }
    
    public Guid SenderId { get; set; }

    public List<Guid> ReceiversIds { get; set; } = [];

    public List<Guid> ReceiversStudyGroupsIds { get; set; } = [];
    
    public List<Uri> Attachments { get; set; } = [];

    public Guid? RelatedClassId { get; set; }
}