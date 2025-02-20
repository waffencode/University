using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class ClassroomRepository : IClassroomRepository
{
    private UserContext Context { get; }
    
    public ClassroomRepository(UserContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(Classroom classroom)
    {
        await Context.Classrooms.AddAsync(classroom);
        await Context.SaveChangesAsync();
    }

    public async Task<Classroom> GetByIdAsync(Guid id)
    {
        return await Context.Classrooms.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<List<Classroom>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Classroom classroom)
    {
        throw new NotImplementedException();
    }
}