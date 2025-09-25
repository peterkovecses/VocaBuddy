namespace Identity.Models;

public class ApplicationUser : IdentityUser
{
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
}