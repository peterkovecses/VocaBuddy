namespace VocaBuddy.Shared.Events.Identity;

public class UserRegistered
{
    public required string Id { get; set; }
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string ConfirmationLink { get; init; }
}