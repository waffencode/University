using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Domain.Model;
using University.Exceptions;
using University.Repository;

namespace University.Service;

public class ScheduleClassService(IScheduleClassRepository scheduleClassRepository, 
    IUserRepository userRepository, 
    IStudyGroupRepository studyGroupRepository, 
    ILogger<ScheduleClassService> logger) : IScheduleClassService
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
        var studentIds = scheduleClassDetailsDto.StudentDetailsDtoList.Select(dto => dto.StudentId).ToList();
        var students = await userRepository.GetUsersByIds(studentIds, cancellationToken);

        var studentDetailsList = scheduleClassDetailsDto.StudentDetailsDtoList.Select(dto => 
        {
            var student = students.FirstOrDefault(s => s.Id == dto.StudentId);
            return new StudentDetails
            {
                Student = student,
                Attendance = dto.Attendance,
                Grade = dto.Grade
            };
        }).ToList();

        entity.Details = new ScheduleClassDetails
        {
            StudentDetailsList = studentDetailsList
        };
        
        await scheduleClassRepository.UpdateEntityAsync(entity, cancellationToken);
        logger.LogInformation("Updated Schedule Class Journal for {Guid}", classId);
    }

    public async Task<List<StudyGroupDto>> GetStudyGroupsForClassAsync(Guid id, CancellationToken cancellationToken)
    {
        var scheduleClass = await scheduleClassRepository.GetAsEntityByIdAsync(id, cancellationToken);
        if (scheduleClass == null)
        {
            throw new EntityNotFoundException($"A schedule class with the ID {id} was not found in the database");
        }
        
        var studyGroupsQueryable = studyGroupRepository.GetAllAsIQueryableAsync();
        
        return await studyGroupsQueryable
            .Where(studyGroup => scheduleClass.Groups.Contains(studyGroup))
            .Select(s => StudyGroupMapper.StudyGroupToStudyGroupDto(s))
            .ToListAsync(cancellationToken);
    }

    public async Task<ScheduleClassDetailsDto> GetScheduleClassDetailsAsync(Guid id, CancellationToken cancellationToken)
    {
        var scheduleClass = await scheduleClassRepository.GetAsEntityByIdAsync(id, cancellationToken);
        if (scheduleClass == null)
        {
            throw new EntityNotFoundException($"A schedule class with the ID {id} was not found in the database");
        }
        
        return ScheduleClassDetailsMapper.ScheduleClassDetailsToScheduleClassDetailsDto(scheduleClass.Details);
    }
}