namespace EmailDispatcher.Helpers;

public static class BusHandlerWrapper
{
    public static Func<T, Task> Handle<T>(
        Func<T, Task> handler, 
        ILogger logger) 
        where T : class, IEvent
    {
        return async message =>
        {
            var messageType = typeof(T).Name;
            var eventId = message.EventId;

            using (logger.BeginScope("MessageType: {MessageType}, EventId: {EventId}", messageType, eventId))
            {
                try
                {
                    logger.LogInformation("Start processing message");
                    await handler(message);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing message");
                }
            }
        };
    }
}