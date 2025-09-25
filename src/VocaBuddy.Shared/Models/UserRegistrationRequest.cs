namespace VocaBuddy.Shared.Models;

public class UserRegistrationRequest
{
    public required string FirstName { get; init; }
    
    public required string LastName { get; init; }
    
    [EmailAddress]
    public required string Email { get; init; } = null!;

    public required string Password { get; init; } = null!;
}
