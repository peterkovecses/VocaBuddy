using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class UserLoginRequest
{
    [EmailAddress]
    public required string Email { get; set; } = default!;
    public required string Password { get; set; } = default!;
}

