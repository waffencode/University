namespace University.Domain;

public class PlannedClass
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public string Theme { get; set; } = string.Empty;
    
    public int Hours { get; set; }
    
    public ClassType ClassType { get; set;}
    
    public required SubjectWorkProgram WorkProgram { get; set; }
}

public class SubjectWorkProgram
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required Subject Subject { get; init; }

    public List<PlannedClass> Classes { get; init; } = [];
}