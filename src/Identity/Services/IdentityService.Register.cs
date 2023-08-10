using Identity.Exceptions;
using Identity.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Identity.Services;

public partial class IdentityService : IIdentityService
{
    public async Task RegisterAsync(string email, string password)
    {
        await ValidateUserAsync(email);
        var user = CreateUser(email);
        await SaveUserAsync(user, password);

        async Task ValidateUserAsync(string email)
        {
            if ((await FindUserByEmailAsync(email)) != null)
            {
                throw new UserExistsException();
            }
        }

        static IdentityUser CreateUser(string email)
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email
            };

        async Task SaveUserAsync(IdentityUser user, string password)
        {
            var result = await _userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidUserRegistrationInputException(MergeIdentityErrors(result.Errors));
            }
        }

        static string MergeIdentityErrors(IEnumerable<IdentityError> errors)
            => string.Join(". ", errors.Select(error => error.Description));
    }
}
