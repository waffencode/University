using System.Diagnostics;
using University.Domain;
using University.Domain.Model;
using University.Repository;

namespace University.Service;

public class ScheduleClassService(IScheduleClassRepository repository, ILogger<ScheduleClassService> logger) : IScheduleClassService
{
    public async Task CreateScheduleClassAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken)
    {
        await repository.AddAsync(scheduleClassDto, cancellationToken);
        var entity = await repository.GetAsEntityByIdAsync(scheduleClassDto.Id, cancellationToken);
        if (entity == null)
        {
            throw new InvalidOperationException("Schedule Class Not Found");
        }

        var newDetailsList = entity.Groups.SelectMany(studyGroup => studyGroup.Students.Select(student =>
            new StudentDetails
        {
            Student = student, 
            Attendance = AttendanceType.Absent, 
            Grade = null
        })).ToList();
        
        entity.Details.StudentDetailsList = newDetailsList;
        
        await repository.UpdateEntityAsync(entity, cancellationToken);
        logger.LogInformation("Created Schedule Class {Guid}", scheduleClassDto.Id);
    }
    
    
}