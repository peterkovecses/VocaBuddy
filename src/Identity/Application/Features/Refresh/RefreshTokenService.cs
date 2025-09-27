namespace Identity.Application.Features.Refresh;

public class RefreshTokenService(
    IdentityContext context,
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
    : IRefreshTokenService
{
    public async Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken)
    {
        var claimPrincipal = GetPrincipalFromToken();
        ValidatePrincipal(claimPrincipal);
        var storedRefreshToken = await GetStoredRefreshTokenAsync();
        ValidateRefreshToken();
        await MakeTokenUsedUpAsync();
        var user = await userManager.FindByIdAsync(claimPrincipal.Claims.Single(claim => claim.Type == "id").Value);

        return await tokenService.CreateSuccessfulAuthenticationResultAsync(user!);

        ClaimsPrincipal GetPrincipalFromToken() => tokenService.GetClaimsOrThrow(token);

        void ValidatePrincipal(ClaimsPrincipal principal)
        {
            var expiryDateUnix =
                long.Parse(principal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                throw new NotExpiredTokenException();
            }
        }

        async Task<CustomRefreshToken?> GetStoredRefreshTokenAsync()
            => await context.RefreshTokens.SingleOrDefaultAsync(r => r.Token == refreshToken);

        void ValidateRefreshToken()
        {
            if (storedRefreshToken == null)
                throw new RefreshTokenNotExistsException();

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
                throw new ExpiredRefreshTokenException();

            if (storedRefreshToken.Invalidated)
                throw new InvalidatedRefreshTokenException();

            if (storedRefreshToken.Used)
                throw new UsedUpRefreshTokenException();

            var jti = claimPrincipal.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.JwtId != jti)
                throw new JwtIdNotMatchException();
        }

        async Task MakeTokenUsedUpAsync()
        {
            storedRefreshToken!.Used = true;
            await context.SaveChangesAsync();
        }
    }
}