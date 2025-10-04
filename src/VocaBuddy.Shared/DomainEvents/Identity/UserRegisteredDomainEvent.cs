namespace VocaBuddy.Shared.DomainEvents.Identity;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public Guid EventId { get; } = Guid.NewGuid(); 
    public required string Email { get; init; }
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    public required string ConfirmationLink { get; init; }
}