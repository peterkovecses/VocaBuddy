namespace VocaBuddy.Shared.Models;

public class UserLoginRequest
{
    [EmailAddress]
    [Required]
    public string Email { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}

