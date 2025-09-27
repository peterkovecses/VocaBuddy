namespace Identity.Application.IdentityFacade;

public class IdentityService(
    IRegistrationService registrationService,
    ILoginService loginService,
    IRefreshTokenService refreshTokenService) : IIdentityService
{
    public async Task RegisterAsync(UserRegistrationRequest request) 
        => await registrationService.RegisterAsync(request);

    public async Task<TokenHolder> LoginAsync(string email, string password,  CancellationToken cancellationToken) 
        => await loginService.LoginAsync(email, password, cancellationToken);

    public async Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken) 
        => await refreshTokenService.RefreshTokenAsync(token, refreshToken, cancellationToken);
}