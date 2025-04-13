namespace Identity.Services;

public partial class IdentityService(UserManager<IdentityUser> userManager,
        IdentityContext context,
        IOptions<CustomTokenValidationParameters> options)
    : IIdentityService
{
    private readonly CustomTokenValidationParameters _tokenValidationParameters = options.Value;
    private readonly JwtSecurityTokenHandler _tokenHandler = new();

    private async Task<TokenHolder> CreateSuccessfulAuthenticationResultAsync(IdentityUser user)
    {
        var key = Encoding.ASCII.GetBytes(_tokenValidationParameters.Secret);
        var tokenDescription = await CreateTokenDescriptionAsync();
        var token = _tokenHandler.CreateToken(tokenDescription);
        var refreshToken = CreateRefreshToken(user, token);
        await SaveRefreshTokenAsync();

        return new TokenHolder { AuthToken = _tokenHandler.WriteToken(token), RefreshToken = refreshToken.Token };

        async Task<SecurityTokenDescriptor> CreateTokenDescriptionAsync()
            => new()
            {
                Subject = new ClaimsIdentity(await GetClaimsAsync()),
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

        async Task SaveRefreshTokenAsync()
        {
            await context.RefreshTokens.AddAsync(refreshToken);
            await context.SaveChangesAsync();
        }

        async Task<List<Claim>> GetClaimsAsync()
        {
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email!),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new("id", user.Id)
            };

            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);
            var userRoles = (await userManager.GetRolesAsync(user))
                                .Select(role => new Claim(ClaimTypes.Role, role));
            claims.AddRange(userRoles);

            return claims;
        }
    }

    private async Task<IdentityUser?> FindUserByEmailAsync(string email)
        => await userManager.FindByNameAsync(email);
}
