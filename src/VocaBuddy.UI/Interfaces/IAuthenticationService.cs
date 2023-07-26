namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task RegisterAsync(UserRegistrationRequest userRegistrationRequest);
    }
}