namespace VocaBuddy.Application.PipelineBehaviors;

public class LoggingBehavior<TRequest, TResponse>(ILogger<LoggingBehavior<TRequest, TResponse>> logger)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var startTime = Stopwatch.GetTimestamp();
        logger.LogInformation(
            "Starting request {RequestName}", 
            typeof(TRequest).Name);

        var result = await next();

        if (result.IsFailure)
        {
            logger.LogError(
                "Request failure {RequestName}, {@Error}",
                typeof(TRequest).Name,
                result.ErrorInfo);
        }

        var elapsedTime = Stopwatch.GetElapsedTime(startTime);
        logger.LogInformation(
            "Completed request {RequestName}, ElapsedTime: {ElapsedTime}",
            typeof(TRequest).Name,
            elapsedTime);

        return result;
    }
}
