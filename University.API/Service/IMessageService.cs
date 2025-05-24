using University.Domain;

namespace University.Service;

public interface IMessageService
{
    public Task<List<User>> GetAvailableReceiversForUserAsync(Guid userId, CancellationToken cancellationToken = default);
}