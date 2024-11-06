namespace Identity.Services;

public partial class IdentityService
{
    public async Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken)
    {
        var claimPrincipal = GetPrincipalFromToken();
        ValidatePrincipal();
        var storedRefreshToken = await GetStoredRefreshTokenAsync();
        ValidateRefreshToken();
        await MakeTokenUsedUpAsync();
        var user = await FindUserByIdAsync();

        return await CreateSuccessfulAuthenticationResultAsync(user!);

        ClaimsPrincipal GetPrincipalFromToken()
        {
            ClaimsPrincipal? principal;
            SecurityToken validatedToken;
            var tokenValidationParameters = _tokenValidationParameters;
            tokenValidationParameters.ValidateLifetime = false;

            try
            {
                principal = _tokenHandler.ValidateToken(token, tokenValidationParameters.ToTokenValidationParameters(), out validatedToken);
            }
            catch
            {
                throw new InvalidJwtException();
            }

            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            {
                throw new InvalidJwtException();
            }

            return principal;
        }

        void ValidatePrincipal()
        {
            var expiryDateUnix =
                long.Parse(claimPrincipal.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Exp).Value);

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
            {
                throw new RefreshTokenNotExistsException();
            }

            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                throw new ExpiredRefreshTokenException();
            }

            if (storedRefreshToken.Invalidated)
            {
                throw new InvalidatedRefreshTokenException();
            }

            if (storedRefreshToken.Used)
            {
                throw new UsedUpRefreshTokenException();
            }

            var jti = claimPrincipal.Claims.Single(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
            if (storedRefreshToken.JwtId != jti)
            {
                throw new JwtIdNotMatchException();
            }
        }

        async Task MakeTokenUsedUpAsync()
        {
            storedRefreshToken!.Used = true;
            await context.SaveChangesAsync();
        }

        static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
            => validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);

        async Task<IdentityUser?> FindUserByIdAsync()
            => await userManager.FindByIdAsync(claimPrincipal.Claims.Single(claim => claim.Type == "id").Value);
    }
}
