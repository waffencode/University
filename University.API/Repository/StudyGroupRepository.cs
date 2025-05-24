using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Infrastructure;

namespace University.Repository;

public class StudyGroupRepository(UniversityContext context) : IStudyGroupRepository
{
    public async Task AddAsync(StudyGroupDto studyGroupDto, CancellationToken cancellationToken = default)
    {
        var fieldOfStudy = await context.FieldsOfStudy.FindAsync([studyGroupDto.FieldOfStudyId], cancellationToken);
        if (fieldOfStudy is null)
        {
            return;
        }

        var existingStudentsList = await context.Users.Where(p => studyGroupDto.StudentsIdList.Contains(p.Id))
            .ToListAsync(cancellationToken);
        if (existingStudentsList.Count != studyGroupDto.StudentsIdList.Count())
        {
            return;
        }
        
        var entity = new StudyGroup
        {
            Id = studyGroupDto.Id,
            Name = studyGroupDto.Name,
            FieldOfStudy = fieldOfStudy,
            Students = existingStudentsList
        };
        
        await context.StudyGroups.AddAsync(entity, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task<StudyGroupDto?> GetByIdAsync(Guid id)
    {
        return StudyGroupMapper.StudyGroupToStudyGroupDto(await context.StudyGroups.AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.FieldOfStudy)
            .Include(p => p.Students)
            .FirstOrDefaultAsync(x => x.Id == id));
    }

    public async Task<IEnumerable<StudyGroupDto>> GetAllAsync()
    {
        return await context.StudyGroups.AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.FieldOfStudy)
            .Include(p => p.Students)
            .Select(s => StudyGroupMapper.StudyGroupToStudyGroupDto(s)).ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await context.StudyGroups.FindAsync(id);
        if (entity is null)
        {
            return;
        }
        
        context.StudyGroups.Remove(entity);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudyGroupDto entity)
    {
        throw new NotImplementedException();
        // context.StudyGroups.Update(entity);
        // await context.SaveChangesAsync();
    }

    public IQueryable<StudyGroup> GetAllAsIQueryable()
    {
        return context.StudyGroups.AsNoTracking()
            .AsSplitQuery()
            .Include(p => p.FieldOfStudy)
            .Include(p => p.Students)
            .AsQueryable();
    }
}