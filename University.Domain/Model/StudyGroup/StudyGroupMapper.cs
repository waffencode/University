namespace University.Domain;

public class StudyGroupMapper
{
    public static StudyGroupDto StudyGroupToStudyGroupDto(StudyGroup entity)
    {
        return new StudyGroupDto
        {
            Id = entity.Id,
            Name = entity.Name,
            FieldOfStudyId = entity.FieldOfStudy.Id,
            StudentsIdList = entity.Students.Select(s => s.Id)
        };
    }
}