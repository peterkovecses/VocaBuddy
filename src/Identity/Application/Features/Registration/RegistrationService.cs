namespace Identity.Application.Features.Registration;

public class RegistrationService(
    UserManager<ApplicationUser> userManager)
    : IRegistrationService
{
    public async Task RegisterAsync(UserRegistrationRequest request)
    {
        if (await userManager.FindByEmailAsync(request.Email) is not null)
        {
            throw new UserExistsException();
        }

        var user = new ApplicationUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Id = Guid.NewGuid().ToString(),
            Email = request.Email,
            UserName = request.Email,
        };

        var result = await userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new InvalidUserRegistrationInputException(MergeIdentityErrors(result.Errors));
        }
    }

    private static string MergeIdentityErrors(IEnumerable<IdentityError> errors)
        => string.Join(". ", errors.Select(error => error.Description));
}