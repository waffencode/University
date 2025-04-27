namespace University.Domain;

public enum ClassroomType
{
    LectureHall,
    LectureRoom,
    GeneralPurpose,
    Laboratory
}

public class Classroom
{
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// The location of the classroom in the university building.
    /// </summary>
    public string Designation { get; set; } = string.Empty;
    
    /// <summary>
    /// The purpose of the classroom or a description if it's a specialized laboratory.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    public ClassroomType Type { get; set; }
}