using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.UI.Models;

public class UserLoginRequest
{
    [EmailAddress]
    [Required(ErrorMessage = "Email Address is required.")]
    public string Email { get; set; } = default!;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = default!;
}
