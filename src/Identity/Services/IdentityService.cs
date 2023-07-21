using Identity.Data;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VocaBuddy.Shared.Models;

namespace Identity.Services;

public partial class IdentityService : IIdentityService
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IdentityContext _context;
    private readonly CustomTokenValidationParameters _tokenValidationParameters;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public IdentityService(
        UserManager<IdentityUser> userManager,
        IdentityContext context,
        IOptions<CustomTokenValidationParameters> options)
    {
        _userManager = userManager;
        _context = context;
        _tokenValidationParameters = options.Value;
        _tokenHandler = new();
    }

    async Task<AuthenticationResult> CreateSuccessfulAuthenticationResultAsync(IdentityUser user)
    {
        var key = Encoding.ASCII.GetBytes(_tokenValidationParameters.Secret);
        var tokenDescription = await CreateTokenDescriptionAsync(user, key);
        var token = _tokenHandler.CreateToken(tokenDescription);
        var refreshToken = CreateRefreshToken(user, token);
        await SaveRefreshTokenAsync(refreshToken);

        return new AuthenticationResult() { Token = _tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };

        async Task<SecurityTokenDescriptor> CreateTokenDescriptionAsync(IdentityUser user, byte[] key)
            => new()
            {
                Subject = new ClaimsIdentity(await GetClaimsAsync(user)),
                Issuer = _tokenValidationParameters.ValidIssuer,
                Audience = _tokenValidationParameters.ValidAudience,
                Expires = DateTime.UtcNow.Add(TimeSpan.Parse(_tokenValidationParameters.TokenLifeTime)),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

        static CustomRefreshToken CreateRefreshToken(IdentityUser user, SecurityToken token)
            => new()
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

        async Task SaveRefreshTokenAsync(CustomRefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        async Task<List<Claim>> GetClaimsAsync(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            var userRoles = (await _userManager.GetRolesAsync(user))
                                .Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(userRoles);

            return claims;
        }
    }

    private async Task<IdentityUser?> FindUserByEmailAsync(string email)
        => await _userManager.FindByNameAsync(email);
}
