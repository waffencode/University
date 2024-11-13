using University.Domain;

namespace University.Security;

public interface IJwtTokenProvider
{
    public string GenerateJwtToken(User user);
}