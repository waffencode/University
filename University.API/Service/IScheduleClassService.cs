using University.Domain;
using University.Domain.Model;

namespace University.Service;

public interface IScheduleClassService
{
    public Task CreateScheduleClassAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken);

    public Task UpdateScheduleClassJournalAsync(Guid classId, ScheduleClassDetailsDto scheduleClassDetailsDto,
        CancellationToken cancellationToken);

    Task<List<StudyGroupDto>> GetStudyGroupsForClassAsync(Guid id, CancellationToken cancellationToken);
    
    Task<ScheduleClassDetailsDto> GetScheduleClassDetailsAsync(Guid id, CancellationToken cancellationToken);
}