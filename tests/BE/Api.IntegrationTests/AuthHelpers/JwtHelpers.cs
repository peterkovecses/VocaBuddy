using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.IntegrationTests.AuthHelpers;

public static class JwtHelpers
{
    public static string GenerateToken(object userId)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConstants.SecurityKey)); 
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        
        var claims = new List<Claim>
        {
            new("id", userId.ToString()!),
            new(ClaimTypes.Name, "TestUser"),
            new("custom-claim", "value")
        };

        var token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}