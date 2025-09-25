namespace Identity.Services;

public partial class IdentityService
{
    public async Task RegisterAsync(UserRegistrationRequest request)
    {
        await ValidateUserAsync();
        var user = CreateUser();
        await SaveUserAsync();
        
        return;

        async Task ValidateUserAsync()
        {
            if (await FindUserByEmailAsync(request.Email) is not null)
            {
                throw new UserExistsException();
            }
        }

        ApplicationUser CreateUser()
            => new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                UserName = request.Email,
            };

        async Task SaveUserAsync()
        {
            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                throw new InvalidUserRegistrationInputException(MergeIdentityErrors(result.Errors));
            }
        }

        static string MergeIdentityErrors(IEnumerable<IdentityError> errors)
            => string.Join(". ", errors.Select(error => error.Description));
    }
}
