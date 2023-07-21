using Identity.Exceptions;
using Identity.Interfaces;
using Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services;

public partial class IdentityService : IIdentityService
{
    public async Task<AuthenticationResult> LoginAsync(string email, string password)
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
