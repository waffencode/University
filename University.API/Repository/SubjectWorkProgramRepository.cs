using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Exceptions;
using University.Infrastructure;

namespace University.Repository;

/// <summary>
/// A repository class for <see cref="SubjectWorkProgram"/>. It provides methods to perform CRUD operations
/// on this entity. 
/// </summary>
/// <author>waffencode@gmail.com</author>
public class SubjectWorkProgramRepository : ISubjectWorkProgramRepository
{
    private UniversityContext Context { get; }
    private readonly ILogger<SubjectWorkProgramRepository> _logger;
    
    /// <summary>
    /// Initializes a new instance of <see cref="SubjectWorkProgramRepository"/> 
    /// with dependency-injected <see cref="UniversityContext"/> and <see cref="ILogger"/>.
    /// </summary>
    /// <param name="context">The <see cref="UniversityContext"/> used to perform database operations.</param>
    /// <param name="logger">The <see cref="ILogger"/> used for logging.</param>
    /// <exception cref="ArgumentNullException">
    /// Thrown if <paramref name="context"/> or <paramref name="logger"/> is null.
    /// </exception>
    public SubjectWorkProgramRepository(UniversityContext context, ILogger<SubjectWorkProgramRepository> logger)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddAsync(SubjectWorkProgram entity)
    {
        var existingSubject = await Context.Subjects
            .FirstOrDefaultAsync(x => x.Id == entity.Subject.Id);
        if (existingSubject == null)
        {
            throw new EntityNotFoundException(typeof(Subject), entity.Subject.Id.ToString());
        }
        
        entity.Subject = existingSubject;
    
        await Context.SubjectWorkPrograms.AddAsync(entity);
        await Context.SaveChangesAsync();
    }

    public async Task<SubjectWorkProgram?> GetByIdAsync(Guid id)
    {
         return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<SubjectWorkProgram?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }
    
    public async Task<List<SubjectWorkProgram>> GetAllAsync()
    {
        return await Context.SubjectWorkPrograms
            .Include(x => x.Subject)
            .Include(x => x.Classes)
            .ToListAsync();
    }

    /// <summary>
    /// Asynchronously deletes <see cref="SubjectWorkProgram"/> with given GUID. 
    /// </summary>
    /// <param name="id">GUID of entity to delete.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="NullReferenceException">If an entity with given GUID is not found in database.</exception>
    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entityToDelete = await GetByIdAsync(id, cancellationToken);
        if (entityToDelete is null)
        {
            _logger.LogWarning("Deletion failed: SubjectWorkProgram with {Id} not found.", id);
            throw new EntityNotFoundException($"SubjectWorkProgram entity with ID {id} not found in database while deletion.");
        }
        
        try
        {
            Context.SubjectWorkPrograms.Remove(entityToDelete);
            await Context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Deleted SubjectWorkProgram entity with ID {Id}.", id);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Failed to delete SubjectWorkProgram with {Id}.", id);
            throw;
        }
    }

    public async Task UpdateAsync(SubjectWorkProgram entity)
    {
        var existingEntity = await GetByIdAsync(entity.Id);
        if (existingEntity is null)
        {
            throw new EntityNotFoundException(typeof(SubjectWorkProgram), entity.Id.ToString());
        }

        var existingSubject = await Context.Subjects
                .FirstOrDefaultAsync(x => x.Id == entity.Subject.Id);
        if (existingSubject == null)
        {
            throw new EntityNotFoundException(typeof(Subject), entity.Subject.Id.ToString());
        }
        
        existingEntity.Subject = existingSubject;
        existingEntity.Classes.Clear();
        existingEntity.Classes.AddRange(entity.Classes);
        
        Context.Update(existingEntity);
        await Context.SaveChangesAsync();
    }
}