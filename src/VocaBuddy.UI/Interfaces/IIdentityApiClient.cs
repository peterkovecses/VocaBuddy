using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest);
        Task<Result> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<Result<TokenHolder>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
