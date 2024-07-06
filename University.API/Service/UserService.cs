using University.Repository;
using University.Utility;

namespace University.Service;

public class UserService(IUserRepository userRepository, IJwtTokenProvider jwtTokenProvider) : IUserService
{
    public async Task<string> Login(string email, string passwordHash)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user.PasswordHash != null && user.PasswordHash.Equals(passwordHash))
        {
            return jwtTokenProvider.GenerateJwtToken(user);
        }

        throw new InvalidOperationException("Email or password is wrong");
    }
}