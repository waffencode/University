namespace University.Service;

public interface IUserService
{
    public Task<string> Login(string email, string passwordHash);
    Task Register(string email, string passwordHash);
}