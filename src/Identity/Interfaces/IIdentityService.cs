using VocaBuddy.Shared.Models;

namespace Identity.Interfaces;

public interface IIdentityService
{
    Task RegisterAsync(string email, string password);
    Task<TokenHolder> LoginAsync(string email, string password);
    Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken);
}
