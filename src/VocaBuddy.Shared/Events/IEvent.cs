namespace VocaBuddy.Shared.Events;

public interface IEvent
{
    Guid EventId { get; }
}