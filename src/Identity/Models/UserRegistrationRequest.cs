using System.ComponentModel.DataAnnotations;

namespace Identity.Models;

public class UserRegistrationRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}
