using University.Domain;

namespace University.Repository;

public interface IStudyGroupRepository
{
    public Task AddAsync(StudyGroupDto entity, CancellationToken cancellationToken = default);

    public Task<StudyGroupDto?> GetByIdAsync(Guid id);

    public Task<IEnumerable<StudyGroupDto>> GetAllAsync();

    public Task DeleteAsync(Guid id);

    public Task UpdateAsync(StudyGroupDto entity);

    public IQueryable<StudyGroup> GetAllAsIQueryableAsync();
}