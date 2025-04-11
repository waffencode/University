using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class ClassTimeSlotRepository : IClassTimeSlotRepository
{
    private UniversityContext Context { get; }
    
    public ClassTimeSlotRepository(UniversityContext context) => Context = context ?? throw new ArgumentNullException(nameof(context));
    
    public async Task AddAsync(ClassTimeSlot entity)
    {
        await Context.ClassTimeSlots.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<ClassTimeSlot?> GetByIdAsync(Guid id)
    {
        return await Context.ClassTimeSlots.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<List<ClassTimeSlot>> GetAllAsync()
    {
        return await Context.ClassTimeSlots.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var classTimeSlot = await GetByIdAsync(id);
        if (classTimeSlot != null)
        {
            Context.ClassTimeSlots.Remove(classTimeSlot);
            await Context.SaveChangesAsync();
        }
    }

    public async Task UpdateAsync(ClassTimeSlot entity)
    {
        Context.ClassTimeSlots.Update(entity);
        await Context.SaveChangesAsync();
    }
}