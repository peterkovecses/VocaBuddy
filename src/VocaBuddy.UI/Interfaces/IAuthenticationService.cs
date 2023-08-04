using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task<Result<ErrorInfo>> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
        Task RefreshTokenAsync();
    }
}