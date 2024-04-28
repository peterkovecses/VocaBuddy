using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.UI.Models;

public class UserRegistrationRequestWithPasswordCheck
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = default!;

    public UserRegistrationRequest ConvertToIdentityModel()
        => new() { Email = this.Email, Password = this.Password };
}
