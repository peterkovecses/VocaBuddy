namespace VocaBuddy.UI.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Result<TokenHolder, IdentityError>> LoginAsync(UserLoginRequest loginRequest);
        Task LogoutAsync();
        Task<Result<TokenHolder, IdentityError>> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest);
        Task RefreshTokenAsync();
    }
}