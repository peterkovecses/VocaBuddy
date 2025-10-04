namespace EmailDispatcher.Models;

public class SmtpSettings
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public int DisconnectDelaySeconds { get; init; }
    public int RetryAttempts { get; init; }
    public int RetryDelaySeconds { get; init; }
    public int MaxCircuitBreakerFailures { get; init; }
    public long CircuitBreakerDurationSeconds { get; init; }
}