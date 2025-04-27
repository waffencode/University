using System.Text;
using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Domain.Model;
using University.Exceptions;
using University.Infrastructure;

namespace University.Repository;

/// <inheritdoc />
public class ScheduleClassRepository(UniversityContext context, ILogger<ScheduleClassRepository> logger) : IScheduleClassRepository
{
    /// <inheritdoc cref="IScheduleClassRepository.AddAsync" />
    /// <remarks>Method logs all the errors and warnings by itself.</remarks>
    /// <exception cref="EntityNotFoundException">Thrown if any of the objects with specified IDs are not in the database.</exception>
    /// <exception cref="Exception">Thrown if any other error occurs during insertion of the entity.</exception>
    public async Task AddAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken = default)
    {
        // Fetch all the dependent objects of the scheduleClassDto from the database if they are present.
        // We pass the keyValues as an array here because of the FindAsync behaviour.
        var teacher = await context.Users.FindAsync([scheduleClassDto.TeacherId], cancellationToken);
        if (teacher is null)
        {
            // Throw EntityNotFoundException if entities weren't found.
            throw new EntityNotFoundException($"User (teacher) with the ID {scheduleClassDto.TeacherId} was not found in the database.");
        }

        var timeSlot = await context.ClassTimeSlots.FindAsync([scheduleClassDto.TimeSlotId], cancellationToken);
        if (timeSlot is null)
        {
            throw new EntityNotFoundException($"ClassTimeSlot with the ID {scheduleClassDto.TimeSlotId} not found in the database.");
        }

        var classroom = await context.Classrooms.FindAsync([scheduleClassDto.ClassroomId], cancellationToken);
        if (classroom is null)
        {
            throw new EntityNotFoundException($"Classroom with the ID {scheduleClassDto.ClassroomId} not found in the database.");
        }

        var subjectWorkProgram =
            await context.SubjectWorkPrograms.FindAsync([scheduleClassDto.SubjectWorkProgramId], cancellationToken);
        if (subjectWorkProgram is null)
        {
            throw new EntityNotFoundException(
                $"SubjectWorkProgram with the ID {scheduleClassDto.SubjectWorkProgramId} was not found in the database.");
        }

        var existingStudyGroups = await context.StudyGroups
            .Where(g => scheduleClassDto.GroupsId.Contains(g.Id))
            .ToListAsync(cancellationToken);
        if (existingStudyGroups.Count != scheduleClassDto.GroupsId.Count())
        {
            var missingIds = scheduleClassDto.GroupsId.Except(existingStudyGroups.Select(g => g.Id));
            var missingIdsListString = new StringBuilder().AppendJoin(", ", missingIds).ToString();
            throw new EntityNotFoundException(
                $"StudyGroups with IDs {missingIdsListString} not found in the database.");
        }
        
        // Create a database entity.
        var scheduleClassEntity = new ScheduleClass
        {
            Id = scheduleClassDto.Id,
            Classroom = classroom,
            ClassType = scheduleClassDto.ClassType,
            Date = scheduleClassDto.Date,
            Groups = existingStudyGroups,
            Name = scheduleClassDto.Name,
            SubjectWorkProgram = subjectWorkProgram,
            Teacher = teacher,
            TimeSlot = timeSlot,
            Details = new ScheduleClassDetails()
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
            logger.LogError(exception,"Could not create the schedule class with the ID {Id}.", scheduleClassEntity.Id);
            throw;
        }
    }

    public async Task<ScheduleClassDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await context.ScheduleClasses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.SubjectWorkProgram)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .Include(c => c.Details)
            .FirstOrDefaultAsync(c => c.Id.Equals(id), cancellationToken);
        return result is not null ? ScheduleClassMapper.ScheduleClassToScheduleClassDto(result) : null;
    }

    public async Task<IEnumerable<ScheduleClassDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await context.ScheduleClasses
            .AsNoTracking()
            .AsSplitQuery()
            .Include(c => c.Classroom)
            .Include(c => c.Groups)
            .Include(c => c.SubjectWorkProgram)
            .Include(c => c.Teacher)
            .Include(c => c.TimeSlot)
            .Include(c => c.Details)
            .Select(sc => ScheduleClassMapper.ScheduleClassToScheduleClassDto(sc))
            .ToListAsync(cancellationToken);
        
        return result;
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