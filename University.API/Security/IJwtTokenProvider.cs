using University.Domain;

namespace University.Utility;

public interface IJwtTokenProvider
{
    public string GenerateJwtToken(User user);
}