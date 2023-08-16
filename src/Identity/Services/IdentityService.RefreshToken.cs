using Identity.Exceptions;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VocaBuddy.Shared.Models;

namespace Identity.Services;

public partial class IdentityService : IIdentityService
{
    public async Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken)
    {
        var claimPrincipal = GetPrincipalFromToken(token);
        ValidatePrincipal(claimPrincipal);
        var storedRefreshToken = await GetStoredRefreshTokenAsync(refreshToken);
        ValidateRefreshToken(claimPrincipal, storedRefreshToken);
        await MakeTokenUsedUpAsync(storedRefreshToken!);
        var user = await FindUserByIdAsync(claimPrincipal);

        return await CreateSuccessfulAuthenticationResultAsync(user!);

        ClaimsPrincipal GetPrincipalFromToken(string token)
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

        void ValidatePrincipal(ClaimsPrincipal claimPrincipal)
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

        async Task<CustomRefreshToken?> GetStoredRefreshTokenAsync(string refreshToken)
            => await _context.RefreshTokens.SingleOrDefaultAsync(r => r.Token == refreshToken);

        void ValidateRefreshToken(ClaimsPrincipal claimPrincipal, CustomRefreshToken? storedRefreshToken)
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

        async Task MakeTokenUsedUpAsync(CustomRefreshToken storedRefreshToken)
        {
            storedRefreshToken.Used = true;
            await _context.SaveChangesAsync();
        }

        static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
            => validatedToken is JwtSecurityToken jwtSecurityToken &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);

        async Task<IdentityUser?> FindUserByIdAsync(ClaimsPrincipal claimPrincipal)
            => await _userManager.FindByIdAsync(claimPrincipal.Claims.Single(claim => claim.Type == "id").Value);
    }
}
