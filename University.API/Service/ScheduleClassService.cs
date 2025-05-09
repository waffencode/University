using University.Domain;
using University.Domain.Model;
using University.Exceptions;
using University.Repository;

namespace University.Service;

public class ScheduleClassService(IScheduleClassRepository scheduleClassRepository, IUserRepository userRepository, ILogger<ScheduleClassService> logger) : IScheduleClassService
{
    public async Task CreateScheduleClassAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken)
    {
        await scheduleClassRepository.AddAsync(scheduleClassDto, cancellationToken);
        var entity = await scheduleClassRepository.GetAsEntityByIdAsync(scheduleClassDto.Id, cancellationToken);
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
        
        await scheduleClassRepository.UpdateEntityAsync(entity, cancellationToken);
        logger.LogInformation("Created Schedule Class {Guid}", scheduleClassDto.Id);
    }

    public async Task UpdateScheduleClassJournalAsync(Guid classId, ScheduleClassDetailsDto scheduleClassDetailsDto,
        CancellationToken cancellationToken)
    {
        var entity = await scheduleClassRepository.GetAsEntityByIdAsync(classId, cancellationToken);
        if (entity == null)
        {
            throw new InvalidOperationException($"Schedule Class with ID {classId} not found");
        }
        
        // We assume that the list of students is complete and up-to-date.
        var studentDetailsTasks = scheduleClassDetailsDto.StudentDetailsDtoList.Select(async dto => new StudentDetails
        {
            Student = await userRepository.GetUserById(dto.StudentId),
            Attendance = dto.Attendance,
            Grade = dto.Grade
        }).ToList();

        await Task.WhenAll(studentDetailsTasks);

        entity.Details = new ScheduleClassDetails
        {
            StudentDetailsList = studentDetailsTasks.Select(task => task.Result).ToList()
        };
        
        await scheduleClassRepository.UpdateEntityAsync(entity, cancellationToken);
        logger.LogInformation("Updated Schedule Class Journal for {Guid}", classId);
    }
}