namespace University.Domain;

public class StudyGroup
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;

    public List<User> Students { get; init; } = [];
}