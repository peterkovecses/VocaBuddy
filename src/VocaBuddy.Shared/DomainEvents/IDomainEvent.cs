namespace VocaBuddy.Shared.DomainEvents;

public interface IDomainEvent
{
    Guid EventId { get; }
}