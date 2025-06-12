using University.Domain;

namespace University.Repository;

public interface IUserRepository
{
    public Task CreateUser(User user);

    public Task<List<User>> GetAllUsers();
    
    public Task<User?> GetUserById(Guid id);
    
    public Task<List<User>> GetUsersByIds(List<Guid> ids, CancellationToken cancellationToken);
    
    public Task<bool> IsUserExist(Guid id);
    
    public Task DeleteUser(Guid id);
    
    public Task UpdateUserFully(Guid id, User user);
    
    public Task UpdateUserPartially(Guid id, User user);
    
    public Task<User> GetUserByEmail(string email);

    public IQueryable<User> GetAllAsIQueryable();

    public Task<List<User>> GetUnauthorisedUsers();
    
    public Task<IEnumerable<User>> FindUserByName(string nameSubstring, CancellationToken cancellationToken);
}
