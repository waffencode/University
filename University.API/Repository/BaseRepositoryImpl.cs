using Microsoft.EntityFrameworkCore;
using University.Infrastructure;

namespace University.Repository;

/// <summary>
/// Implementation of a basic CRUD repository to store simple entities without complex relationships.
/// Requires inheritance of the GetByIdAsync method at a minimum.
/// </summary>
/// <typeparam name="T">The entity type</typeparam>
/// <author>waffencode@gmail.com</author>
public abstract class BaseRepositoryImpl<T>(UniversityContext context) : IBaseRepository<T>
    where T : class
{
    protected UniversityContext Context { get; } = context;
    protected DbSet<T> Set { get; } = context.Set<T>();

    public async Task AddAsync(T entity)
    {
        await Set.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public abstract Task<T?> GetByIdAsync(Guid id);

    public async Task<List<T>> GetAllAsync()
    {
        return await Set.ToListAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        Set.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        Set.Update(entity);
        await Context.SaveChangesAsync();
    }
}