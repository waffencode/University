using University.Domain;

namespace University.Repository;

public interface ISubjectWorkProgramRepository : IBaseRepository<SubjectWorkProgram>
{
    // To disable implementation without cancellation token.
    Task IBaseRepository<SubjectWorkProgram>.DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}