namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task RegisterAsync(UserRegistrationRequest userRegistrationRequest);
    }
}