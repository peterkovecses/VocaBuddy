namespace Identity.Application.Refresh;

public interface IRefreshTokenService
{
    Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken);
}