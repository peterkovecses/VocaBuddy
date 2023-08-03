using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<Result<TokenHolder, BaseError>> LoginAsync(UserLoginRequest loginRequest);
        Task<Result<BaseError>> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<Result<TokenHolder, BaseError>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
