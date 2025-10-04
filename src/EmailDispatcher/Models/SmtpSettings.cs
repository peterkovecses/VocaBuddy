namespace EmailDispatcher.Models;

public class SmtpSettings
{
    public required string Host { get; init; }
    public int Port { get; init; }
    public int DisconnectDelaySeconds { get; init; }
    public ResiliencySettings Resiliency { get; init; } = new();
}