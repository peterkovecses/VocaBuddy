namespace Identity.Services;

public partial class IdentityService
{
    public async Task RegisterAsync(string email, string password)
    {
        await ValidateUserAsync();
        var user = CreateUser();
        await SaveUserAsync();
        
        return;

        async Task ValidateUserAsync()
        {
            if ((await FindUserByEmailAsync(email)) != null)
            {
                throw new UserExistsException();
            }
        }

        IdentityUser CreateUser()
            => new()
            {
                Id = Guid.NewGuid().ToString(),
                Email = email,
                UserName = email
            };

        async Task SaveUserAsync()
        {
            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
            {
                throw new InvalidUserRegistrationInputException(MergeIdentityErrors(result.Errors));
            }
        }

        static string MergeIdentityErrors(IEnumerable<IdentityError> errors)
            => string.Join(". ", errors.Select(error => error.Description));
    }
}
