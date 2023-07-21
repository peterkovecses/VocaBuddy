using VocaBuddy.Shared.Models;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest);
        Task RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
