namespace Identity.Application.Features.Refresh;

public interface IRefreshTokenService
{
    Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
}