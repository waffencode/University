using University.Domain;

namespace University.Repository;

public interface IClassroomRepository
{
    public Task AddAsync(Classroom classroom);

    public Task<Classroom?> GetByIdAsync(Guid id);
    
    public Task<List<Classroom>> GetAllAsync();
    
    public Task DeleteAsync(Guid id);

    public Task UpdateAsync(Classroom classroom);
}