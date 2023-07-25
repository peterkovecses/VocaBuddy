namespace VocaBuddy.Infrastructure.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
