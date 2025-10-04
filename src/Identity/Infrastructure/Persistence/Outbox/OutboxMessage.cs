namespace Identity.Infrastructure.Persistence.Outbox;

internal sealed record OutboxMessage(
    Guid Id,
    string Name,
    string Content,
    DateTime? CreatedOnUtc = null,
    string? Error = null);