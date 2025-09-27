namespace Identity.Application.Features.Login;

public class LoginService(
    UserManager<ApplicationUser> userManager,
    ITokenService tokenService)
    : ILoginService
{
    public async Task<TokenHolder> LoginAsync(string email, string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null || !await userManager.CheckPasswordAsync(user, password))
        {
            throw new InvalidCredentialsException();
        }

        return await tokenService.CreateSuccessfulAuthenticationResultAsync(user);
    }
}