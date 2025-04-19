using University.Domain;

namespace University.Repository;

/// <summary>
/// Represents a repository to perform database operations on <see cref="ScheduleClass"/> entity.
/// Uses DTOs as an output types. Supports <see cref="CancellationToken"/> in asynchronous methods.
/// </summary>
/// <author>waffencode@gmail.com</author>
public interface IScheduleClassRepository
{
    /// <summary>
    /// Asynchronously creates a database entity from the specified DTO and inserts it into the database.
    /// </summary>
    /// <param name="scheduleClassDto">An instance of the <see cref="ScheduleClassDto"/> which will be converted into a <see cref="ScheduleClass"/> and inserted into the database.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/> to cancel create operation.</param>
    /// <returns></returns>
    public Task AddAsync(ScheduleClassDto scheduleClassDto, CancellationToken cancellationToken = default);

    public Task<IEnumerable<ScheduleClassDto>> GetAllAsync(CancellationToken cancellationToken = default);
}