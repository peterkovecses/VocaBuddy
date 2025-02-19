namespace VocaBuddy.UI.Interfaces;

public interface IAuthenticationService
{
    Task<Result> LoginAsync(UserLoginRequest loginRequest, CancellationToken cancellationToken);
    Task LogoutAsync(CancellationToken cancellationToken);
    Task<Result> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest, CancellationToken cancellationToken);
    Task RefreshTokenAsync(CancellationToken cancellationToken);
    Task<bool> IsUserAuthenticatedAsync();
}