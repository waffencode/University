using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class ScheduleClassRepository(UserContext context) : BaseRepositoryImpl<ScheduleClass>(context), IScheduleClassRepository
{
    public override async Task<ScheduleClass?> GetByIdAsync(Guid id)
    {
        return await Set
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));
    }

    public new async Task<List<ScheduleClass>> GetAllAsync()
    {
        return await Set
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.Subject)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .ToListAsync();
    }
}