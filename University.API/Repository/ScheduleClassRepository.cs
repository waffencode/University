using System.Text;
using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Exceptions;
using University.Infrastructure;

namespace University.Repository;

public class ScheduleClassRepository(UniversityContext context, ILogger<ScheduleClassRepository> logger) : IScheduleClassRepository
{
    public async Task AddAsync(ScheduleClassDto entity, CancellationToken cancellationToken)
    {
        // Fetch all the dependent objects of the entity from the database if they are present.
        // We pass the keyValues as an array here because of the FindAsync behaviour.
        var teacher = await context.Users.FindAsync([entity.TeacherId], cancellationToken);
        if (teacher is null)
        {
            // Throw EntityNotFoundException if entities weren't found.
            throw new EntityNotFoundException($"User (teacher) with the ID {entity.TeacherId} was not found in the database.");
        }

        var timeSlot = await context.ClassTimeSlots.FindAsync([entity.TimeSlotId], cancellationToken);
        if (timeSlot is null)
        {
            throw new EntityNotFoundException($"ClassTimeSlot with the ID {entity.TimeSlotId} not found in the database.");
        }

        var classroom = await context.Classrooms.FindAsync([entity.ClassroomId], cancellationToken);
        if (classroom is null)
        {
            throw new EntityNotFoundException($"Classroom with the ID {entity.ClassroomId} not found in the database.");
        }

        var subjectWorkProgram =
            await context.SubjectWorkPrograms.FindAsync([entity.SubjectWorkProgramId], cancellationToken);
        if (subjectWorkProgram is null)
        {
            throw new EntityNotFoundException(
                $"SubjectWorkProgram with the ID {entity.SubjectWorkProgramId} was not found in the database.");
        }

        var existingStudyGroups = await context.StudyGroups
            .Where(g => entity.GroupsId.Contains(g.Id))
            .ToListAsync(cancellationToken);
        if (existingStudyGroups.Count != entity.GroupsId.Count())
        {
            var missingIds = entity.GroupsId.Except(existingStudyGroups.Select(g => g.Id));
            var missingIdsListString = new StringBuilder().AppendJoin(", ", missingIds).ToString();
            throw new EntityNotFoundException(
                $"StudyGroups with IDs {missingIdsListString} not found in the database.");
        }
        
        // Create a database entity.
        var scheduleClassEntity = new ScheduleClass
        {
            Id = entity.Id,
            Classroom = classroom,
            ClassType = entity.ClassType,
            Date = entity.Date,
            Groups = existingStudyGroups,
            Name = entity.Name,
            SubjectWorkProgram = subjectWorkProgram,
            Teacher = teacher,
            TimeSlot = timeSlot
        };
        
        // Store the entity in the app database.
        try
        {
            await context.AddAsync(scheduleClassEntity, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Created the schedule class with the ID {Id}.", scheduleClassEntity.Id);
        }
        catch (Exception exception)
        {
            logger.LogError("Could not create the schedule class with the ID {Id}. Cause: {Message}", scheduleClassEntity.Id, exception.Message);
            throw;
        }
    }

    public async Task<ScheduleClassDto?> GetByIdAsync(Guid id)
    {
        var result = await context.ScheduleClasses
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.SubjectWorkProgram)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .FirstOrDefaultAsync(c => c.Id.Equals(id));
        return result is not null ? ScheduleClassMapper.ScheduleClassToScheduleClassDto(result) : null;
    }

    public async Task<List<ScheduleClassDto>> GetAllAsync()
    {
        var result = await context.ScheduleClasses
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.SubjectWorkProgram)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .ToListAsync();
        
        return result.Select(ScheduleClassMapper.ScheduleClassToScheduleClassDto).ToList();
    }

    public async Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(ScheduleClassDto entity)
    {
        throw new NotImplementedException();
    }
}