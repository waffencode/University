using Microsoft.EntityFrameworkCore;
using University.Domain;
using University.Exceptions;
using University.Repository;

namespace University.Service;

public class MessageService(IUserRepository userRepository) : IMessageService
{
    public async Task<List<User>> GetAvailableReceiversForUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            throw new EntityNotFoundException("User not found");
        }
        
        var usersQueryable = userRepository.GetAllAsIQueryable();

        return user.Role switch
        {
            UserRole.Student => await usersQueryable.Where(x => x.Role == UserRole.Teacher).ToListAsync(cancellationToken),
            UserRole.Teacher => await usersQueryable.ToListAsync(cancellationToken),
            UserRole.Manager => await usersQueryable.ToListAsync(cancellationToken),
            UserRole.Admin => await usersQueryable.ToListAsync(cancellationToken),
            var role => throw new Exception(
                $"User role {role} is not supported for {nameof(GetAvailableReceiversForUserAsync)}")
        };
    }
}