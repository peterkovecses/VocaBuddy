namespace VocaBuddy.UI.Interfaces
{
    public interface IIdentityApiClient
    {
        Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest, CancellationToken cancellationToken);
        Task<Result> RegisterAsync(UserRegistrationRequest registrationRequest, CancellationToken cancellationToken);
        Task<Result<TokenHolder>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken);
    }
}
