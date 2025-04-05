using System.Diagnostics;
using University.Domain;
using University.Repository;
using University.Security;

namespace University.Service;

public class UserService(IUserRepository userRepository, IRegistrationRequestRepository registrationRequestRepository, IJwtTokenProvider jwtTokenProvider) : IUserService
{
    public async Task<string> Login(string email, string passwordHash)
    {
        var user = await userRepository.GetUserByEmail(email);
        if (user.PasswordHash is not null && !passwordHash.Equals(string.Empty) && user.PasswordHash.Equals(passwordHash))
        {
            return jwtTokenProvider.GenerateJwtToken(user);
        }

        throw new InvalidOperationException("Email or password is wrong");
    }

    public async Task Register(User user)
    {
        await userRepository.CreateUser(user);

        var registrationRequest = new RegistrationRequest
        {
            User = user,
            RequestedRole = UserRole.Student
        };
        
        await registrationRequestRepository.CreateRegistrationRequest(registrationRequest);
    }

    public async Task AuthorizeUser(Guid registrationRequestId)
    {
        var registrationRequest = await registrationRequestRepository.GetRegistrationRequestById(registrationRequestId);
        
        Debug.Assert(registrationRequest is not null, "registrationRequest is null");
        
        var user = registrationRequest.User;
        user.Role = registrationRequest.RequestedRole;
        await userRepository.UpdateUserFully(user.Id, user);
        registrationRequest.Status = RegistrationRequestStatus.Accepted;
        await registrationRequestRepository.UpdateRegistrationRequest(registrationRequest.Id, registrationRequest);
    }

    public async Task<RegistrationRequest> GetUserPendingRegistrationRequestAsync(Guid userId)
    {
        return (await registrationRequestRepository.GetPendingRegistrationRequests())
            .First(r => r.User.Id.Equals(userId));
    }
}