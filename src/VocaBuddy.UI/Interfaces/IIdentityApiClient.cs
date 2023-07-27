namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest);
        Task RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<TokenHolder> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
