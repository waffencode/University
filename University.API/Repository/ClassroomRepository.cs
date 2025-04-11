using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class ClassroomRepository : IClassroomRepository
{
    private UniversityContext Context { get; }
    
    public ClassroomRepository(UniversityContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(Classroom classroom)
    {
        await Context.Classrooms.AddAsync(classroom);
        await Context.SaveChangesAsync();
    }

    public async Task<Classroom?> GetByIdAsync(Guid id)
    {
        return await Context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Classroom>> GetAllAsync()
    {
        return await Context.Classrooms.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        Context.Classrooms.Remove(await GetByIdAsync(id));
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Classroom classroom)
    {
        Context.Classrooms.Update(classroom);
        await Context.SaveChangesAsync();
    }
}