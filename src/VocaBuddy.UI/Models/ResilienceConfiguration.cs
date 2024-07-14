namespace VocaBuddy.UI.Models;

public class ResilienceConfiguration
{
    public int MaxRetryAttempts { get; set; }
    public TimeSpan Delay { get; set; }
    public int BackoffType { get; set; }
    public TimeSpan AttemptTimeout { get; set; }
    public TimeSpan TotalRequestTimeout { get; set; }
    public TimeSpan CircuitBreakerDuration { get; set; }

    public static ResilienceConfiguration CreateDefaultConfiguration()
        => new()
        {
            MaxRetryAttempts = 3,
            Delay = TimeSpan.FromSeconds(2),
            BackoffType = 2,
            AttemptTimeout = TimeSpan.FromSeconds(3),
            TotalRequestTimeout = TimeSpan.FromSeconds(9),
            CircuitBreakerDuration = TimeSpan.FromSeconds(20)
        };
}
