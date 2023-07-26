using System.ComponentModel.DataAnnotations;
using VocaBuddy.UI.Validators;

namespace VocaBuddy.UI.Models;

public class UserRegistrationRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    [PasswordValidation]
    public string Password { get; set; } = default!;

    [Required]
    [Compare(nameof(Password), ErrorMessage = "The passwords do not match.")]
    public string ConfirmPassword { get; set; } = default!;
}
