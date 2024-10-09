namespace Identity.Services;

public partial class IdentityService
{
    public async Task<TokenHolder> LoginAsync(string email, string password)
    {
        var user = await FindUserByEmailAsync(email);
        await ThrowIfInvalidCredentialsAsync();

        return await CreateSuccessfulAuthenticationResultAsync(user!);

        async Task ThrowIfInvalidCredentialsAsync()
        {
            if (user is null || !await userManager.CheckPasswordAsync(user, password))
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}
