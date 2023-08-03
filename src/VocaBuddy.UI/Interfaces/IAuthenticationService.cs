using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<TokenHolder, BaseError>> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task<Result<BaseError>> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
        Task RefreshTokenAsync();
    }
}