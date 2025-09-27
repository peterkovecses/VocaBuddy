namespace Identity.Application.IdentityFacade;

public interface IIdentityService
{
    Task RegisterAsync(UserRegistrationRequest request);
    Task<TokenHolder> LoginAsync(string email, string password, CancellationToken cancellationToken);
    Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken);
}
