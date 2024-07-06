using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using University.Domain;

namespace University.Utility;

public class JwtTokenProvider(IOptions<JwtOptions> options) : IJwtTokenProvider
{
    private readonly JwtOptions _options = options.Value;
    
    public string GenerateJwtToken(User user)
    {
        Claim[] claims = [new Claim("userId", user.Id.ToString())];
        
        var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)), 
        SecurityAlgorithms.HmacSha256);
    
        var token = new JwtSecurityToken(
            claims: claims,
            signingCredentials: signingCredentials,
            expires: DateTime.Now.AddHours(_options.ExpireHours)
        );
        
        var jwtToken  = new JwtSecurityTokenHandler().WriteToken(token);
        
        return jwtToken;
    }
}