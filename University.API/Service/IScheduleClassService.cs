using University.Domain;

namespace University.Service;

public interface IScheduleClassService
{
    public Task CreateScheduleClassAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken);
}