namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
        Task RefreshTokenAsync();
    }
}