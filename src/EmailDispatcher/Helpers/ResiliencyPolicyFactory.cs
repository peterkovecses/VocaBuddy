namespace EmailDispatcher.Helpers;

public class ResiliencyPolicyFactory
{
    public static AsyncPolicyWrap CreatePolicy(ResiliencySettings settings, ILogger<ResiliencyPolicyFactory> logger)
    {
        var circuitBreakerPolicy = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: settings.MaxCircuitBreakerFailures,
                durationOfBreak: TimeSpan.FromSeconds(settings.CircuitBreakerDurationSeconds));
        
        var retryPolicy = Policy
            .Handle<SocketException>()
            .Or<MailKit.Net.Smtp.SmtpCommandException>()
            .Or<MailKit.Net.Smtp.SmtpProtocolException>()
            .WaitAndRetryAsync(
                retryCount: settings.RetryAttempts,
                sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(settings.RetryDelaySeconds, attempt)),
                onRetry: (exception, timespan, retryCount, _) =>
                {
                    logger.LogWarning(exception,
                        "Retry {RetryAttempt} after {Delay}s due to {Message}",
                        retryCount, timespan.TotalSeconds, exception.Message);
                });

        return Policy.WrapAsync(circuitBreakerPolicy, retryPolicy);
    }
}