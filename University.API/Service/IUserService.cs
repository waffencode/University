using University.Domain;

namespace University.Service;

public interface IUserService
{
    public Task<string> Login(string email, string passwordHash);
    
    Task Register(User user);
    
    public Task AuthorizeUser(Guid registrationRequestId);
}