namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task<IdentityResult> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
    }
}