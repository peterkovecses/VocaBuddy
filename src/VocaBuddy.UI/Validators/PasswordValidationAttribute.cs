using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using VocaBuddy.UI.Authentication.Models;

namespace VocaBuddy.UI.Validators;

public class PasswordValidationAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var passwordOptions = validationContext.GetRequiredService<IOptions<PasswordOptions>>().Value;

        if (value is not string password)
        {
            throw new ArgumentException("This attribute can only be used on string type.", nameof(value));
        }

        if (passwordOptions.RequireDigit && !password.Any(char.IsDigit))
        {
            return new ValidationResult("A password must contain at least one digit.");
        }

        if (passwordOptions.RequireLowercase && !password.Any(char.IsLower))
        {
            return new ValidationResult("A password must contain at least one lowercase letter.");
        }

        if (passwordOptions.RequireUppercase && !password.Any(char.IsUpper))
        {
            return new ValidationResult("A password must contain at least one uppercase letter.");
        }

        if (passwordOptions.RequireNonAlphanumeric && !password.Any(ch => !char.IsLetterOrDigit(ch)))
        {
            return new ValidationResult("A password must contain at least one non-alphanumeric character.");
        }

        return ValidationResult.Success!;
    }
}
