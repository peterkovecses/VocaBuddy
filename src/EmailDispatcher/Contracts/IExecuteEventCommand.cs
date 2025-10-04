namespace EmailDispatcher.Contracts;

public interface IExecuteEventCommand
{
    Guid EventId { get; }
}