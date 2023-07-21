using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.UI.Models;

public class UserRegistrationRequest
{
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
}
