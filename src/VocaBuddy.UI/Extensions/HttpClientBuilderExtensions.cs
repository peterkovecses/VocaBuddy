namespace VocaBuddy.UI.Extensions;

public static class HttpClientBuilderExtensions
{
    public static IHttpStandardResiliencePipelineBuilder AddResilience(this IHttpClientBuilder builder, ResilienceConfiguration resilienceConfig)
        => builder.AddStandardResilienceHandler(options =>
        {
            // Customize retry
            options.Retry.MaxRetryAttempts = resilienceConfig.MaxRetryAttempts;
            options.Retry.Delay = resilienceConfig.Delay;
            options.Retry.BackoffType = (DelayBackoffType)resilienceConfig.BackoffType;

            // Customize timeouts
            options.AttemptTimeout.Timeout = resilienceConfig.AttemptTimeout;
            options.TotalRequestTimeout.Timeout = resilienceConfig.TotalRequestTimeout;

            // Customize circuit breaker
            options.CircuitBreaker.BreakDuration = resilienceConfig.CircuitBreakerDuration;            
        });
}
