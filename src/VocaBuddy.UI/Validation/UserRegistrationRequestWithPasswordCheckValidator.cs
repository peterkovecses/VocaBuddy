namespace VocaBuddy.UI.Validation;

public class UserRegistrationRequestWithPasswordCheckValidator : AbstractValidator<UserRegistrationRequestWithPasswordCheck>
{
    public UserRegistrationRequestWithPasswordCheckValidator(IOptions<PasswordConfiguration> options)
    {
        var passwordOptions = options.Value;

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required.")
            .MaximumLength(50)
            .WithMessage("First name cannot be longer than 50 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required.")
            .MaximumLength(50)
            .WithMessage("Last name cannot be longer than 50 characters.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email.");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.")
            .Must(password => !passwordOptions.RequireDigit || password.Any(char.IsDigit))
            .WithMessage("A password must contain at least one digit.")
            .Must(password => !passwordOptions.RequireLowercase || password.Any(char.IsLower))
            .WithMessage("A password must contain at least one lowercase letter.")
            .Must(password => !passwordOptions.RequireUppercase || password.Any(char.IsUpper))
            .WithMessage("A password must contain at least one uppercase letter.")
            .Must(password => !passwordOptions.RequireNonAlphanumeric || password.Any(ch => !char.IsLetterOrDigit(ch)))
            .WithMessage("A password must contain at least one non-alphanumeric character.");

        RuleFor(x => x.ConfirmPassword)
            .Equal(x => x.Password).WithMessage("The passwords do not match.");
    }
}
