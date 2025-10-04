namespace EmailDispatcher.Logging;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IDomainEvent
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        using (LogContext.PushProperty("RequestName", typeof(TRequest).Name))
        using (LogContext.PushProperty("EventId", request.EventId))
        {
            try
            {
                logger.LogInformation("Start processing message.");
                var result = await next(cancellationToken);
                logger.LogInformation("Finished processing message.");
                
                return result;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred while processing message.");
                throw;
            }
        }
    }
}