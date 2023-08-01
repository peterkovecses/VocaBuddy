namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<Result<TokenHolder, IdentityError>> LoginAsync(UserLoginRequest loginRequest);
        Task<Result<TokenHolder, IdentityError>> RegisterAsync(UserRegistrationRequest registrationRequest);
        Task<Result<TokenHolder, IdentityError>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest);
    }
}
