namespace University.Domain;

public enum FormOfStudy
{
    FullTime,
    PartTime,
    DistanceLearning
}

public class FieldOfStudy
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Code { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Specialization { get; set; } = string.Empty;
    
    public FormOfStudy FormOfStudy { get; set; }
}