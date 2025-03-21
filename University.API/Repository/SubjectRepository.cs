using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class SubjectRepository(UserContext context) : BaseRepositoryImpl<Subject>(context), ISubjectRepository
{
    public override async Task<Subject?> GetByIdAsync(Guid id) => await Set.FirstOrDefaultAsync(p => p.Id.Equals(id));
}