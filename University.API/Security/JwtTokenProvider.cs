using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using University.Domain;

namespace University.Security;

/// <summary>
/// Encapsulates functionality for generating JWT tokens.
/// </summary>
/// <param name="options">Options from application settings, required to provide signing credentials.</param>
/// <author>waffencode@gmail.com</author>
public class JwtTokenProvider(IOptions<JwtOptions> options) : IJwtTokenProvider
{
    private readonly JwtOptions _options = options.Value;
    
    /// <summary>
    /// Generates JWT token based on provided user data.
    /// </summary>
    /// <param name="user"><see cref="User" /> object to generate JWT token for.</param>
    /// <returns>Generated JWT token.</returns>
    public string GenerateJwtToken(User user)
    {
        Claim[] claims = [new("userId", user.Id.ToString()), new(ClaimTypes.Role, user.Role.ToString()), new(ClaimTypes.Email, user.Email)];
        
        var signingCredentials = new SigningCredentials(
                key: new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), 
                algorithm: SecurityAlgorithms.HmacSha256);
    
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_options.ExpireHours)
        );
        
        var jwtToken  = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwtToken;
    }
}