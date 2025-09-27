namespace Identity.Application.Services;

public interface ITokenService
{
    ClaimsPrincipal GetClaimsOrThrow(string token);
    Task<TokenHolder> CreateSuccessfulAuthenticationResultAsync(ApplicationUser user);
}