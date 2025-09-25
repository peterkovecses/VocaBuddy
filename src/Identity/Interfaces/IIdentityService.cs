namespace Identity.Interfaces;

public interface IIdentityService
{
    Task RegisterAsync(UserRegistrationRequest request);
    Task<TokenHolder> LoginAsync(string email, string password);
    Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken);
}
