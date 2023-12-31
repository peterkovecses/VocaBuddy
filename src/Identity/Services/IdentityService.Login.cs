using Identity.Exceptions;
using Microsoft.AspNetCore.Identity;
using VocaBuddy.Shared.Models;

namespace Identity.Services;

public partial class IdentityService
{
    public async Task<TokenHolder> LoginAsync(string email, string password)
    {
        var user = await FindUserByEmailAsync(email);
        await ThrowIfInvalidCredentialsAsync(user, password);

        return await CreateSuccessfulAuthenticationResultAsync(user!);

        async Task ThrowIfInvalidCredentialsAsync(IdentityUser? user, string password)
        {
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new InvalidCredentialsException();
            }
        }
    }
}
