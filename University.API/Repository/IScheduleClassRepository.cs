using University.Domain;

namespace University.Repository;

public interface IScheduleClassRepository
{
    public Task AddAsync(ScheduleClassDto entity, CancellationToken cancellationToken);
}