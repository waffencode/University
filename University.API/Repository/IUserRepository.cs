using University.Domain;

namespace University.Repository;

public interface IUserRepository
{
    public Task CreateUser(User user);

    public Task<List<User>> GetAllUsers();
    
    public Task<User?> GetUserById(Guid id);
    
    public Task<bool> IsUserExist(Guid id);
    
    public Task DeleteUser(Guid id);
    
    public Task UpdateUserFully(Guid id, User user);
    
    public Task UpdateUserPartially(Guid id, User user);
    
    public Task<User> GetUserByEmail(string email);

    public Task<List<User>> GetUnauthorisedUsers();
}
