namespace University.Domain;

public class Classroom
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Designation { get; set; } = string.Empty;
}