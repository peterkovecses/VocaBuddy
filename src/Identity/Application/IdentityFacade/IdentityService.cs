namespace Identity.Application.IdentityFacade;

public class IdentityService(
    IRegistrationService registrationService,
    ILoginService loginService,
    IRefreshTokenService refreshTokenService) : IIdentityService
{
    public async Task RegisterAsync(UserRegistrationRequest request) 
        => await registrationService.RegisterAsync(request);

    public async Task<TokenHolder> LoginAsync(string email, string password) 
        => await loginService.LoginAsync(email, password);

    public async Task<TokenHolder> RefreshTokenAsync(string token, string refreshToken) 
        => await refreshTokenService.RefreshTokenAsync(token, refreshToken);
}