using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class SubjectWorkProgramRepository : ISubjectWorkProgramRepository
{
    private UserContext Context { get; }
    
    public SubjectWorkProgramRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(SubjectWorkProgram entity)
    {
        await Context.SubjectWorkPrograms.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<SubjectWorkProgram?> GetByIdAsync(Guid id)
    {
         return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<SubjectWorkProgram>> GetAllAsync()
    {
        return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Context.SubjectWorkPrograms.Remove(await GetByIdAsync(id));
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(SubjectWorkProgram entity)
    {
        throw new NotImplementedException();
    }
}