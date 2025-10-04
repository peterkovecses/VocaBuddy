namespace Identity.Domain.Models;

public class ApplicationUser : IdentityUser, IEntity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public required string FirstName { get; init; }
    public required string LastName { get; init; }
    
    public void AddDomainEvent(IDomainEvent domainDomainEvent) => _domainEvents.Add(domainDomainEvent);
    
    public void AddUserRegisteredEvent(string confirmationLink)
    {
        var userRegisteredEvent = new UserRegisteredDomainEvent
        {
            Email = Email  ?? string.Empty,
            FirstName = FirstName,
            LastName = LastName,
            ConfirmationLink = confirmationLink
        };
        AddDomainEvent(userRegisteredEvent);
    }
    
    public IReadOnlyList<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();

    public void ClearDomainEvents() => _domainEvents.Clear();
}