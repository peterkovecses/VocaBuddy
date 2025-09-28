namespace EmailDispatcher.Helpers;

public static class BusHandlerWrapper
{
    public static Func<T, Task> Handle<T>(
        Func<T, Task> handler, 
        Serilog.ILogger logger) 
        where T : class, IEvent
    {
        return async message =>
        {
            var messageType = typeof(T).Name;
            var eventId = message.EventId;

            using (LogContext.PushProperty("MessageType", messageType))
            using (LogContext.PushProperty("EventId", eventId))
            {
                try
                {
                    logger.Information("Start processing message.");
                    await handler(message);
                }
                catch (Exception ex)
                {
                    logger.Error(ex, "An error occurred while processing message.");
                }
            }
        };
    }
}