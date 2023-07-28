namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest);
        Task<IdentityResult> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<IdentityResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
