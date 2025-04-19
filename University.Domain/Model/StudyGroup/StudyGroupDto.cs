namespace University.Domain;

public class StudyGroupDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public IEnumerable<Guid> StudentsIdList { get; set; }

    public Guid FieldOfStudyId { get; set; }
}