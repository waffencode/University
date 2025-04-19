namespace University.Domain;

public class StudyGroup
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public List<User> Students { get; set; } = [];

    public FieldOfStudy FieldOfStudy { get; set; } = new();
}