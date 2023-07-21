using VocaBuddy.Shared.Models;

namespace Identity.Interfaces;

public interface IIdentityService
{
    Task RegisterAsync(string email, string password);
    Task<AuthenticationResult> LoginAsync(string email, string password);
    Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
}
