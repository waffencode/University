using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class FieldOfStudyRepository(UniversityContext context) : BaseRepositoryImpl<FieldOfStudy>(context), IFieldOfStudyRepository
{
    public override async Task<FieldOfStudy?> GetByIdAsync(Guid id)
    {
        return await Set.FirstOrDefaultAsync(f => f.Id.Equals(id));
    }
}