using VocaBuddy.UI.Authentication;

namespace VocaBuddy.UI.Interfaces;

public interface IAuthenticationService
{
    Task<Result> LoginAsync(UserLoginRequest loginRequest);
    Task LogoutAsync();
    Task<Result> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
    Task RefreshTokenAsync();
}