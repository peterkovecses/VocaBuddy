namespace EmailDispatcher.Models;

public class ResiliencySettings
{
    public int RetryAttempts { get; init; }
    public int RetryDelaySeconds { get; init; }
    public int MaxCircuitBreakerFailures { get; init; }
    public long CircuitBreakerDurationSeconds { get; init; }
}