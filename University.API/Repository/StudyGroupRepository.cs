using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class StudyGroupRepository : IStudyGroupRepository
{
    private UserContext Context { get; }

    public StudyGroupRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(StudyGroup entity)
    {
        await Context.StudyGroups.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<StudyGroup?> GetByIdAsync(Guid id)
    {
        return await Context.StudyGroups.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<StudyGroup>> GetAllAsync()
    {
        return await Context.StudyGroups.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Context.StudyGroups.Remove(await GetByIdAsync(id));
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudyGroup entity)
    {
        Context.StudyGroups.Update(entity);
        await Context.SaveChangesAsync();
    }
}