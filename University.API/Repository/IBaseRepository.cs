namespace University.Repository;

public interface IBaseRepository<T> where T : class
{
    public Task AddAsync(T entity);

    public Task<T?> GetByIdAsync(Guid id);
    
    public Task<List<T>> GetAllAsync();
    
    public Task DeleteAsync(Guid id);

    public Task UpdateAsync(T entity);
}