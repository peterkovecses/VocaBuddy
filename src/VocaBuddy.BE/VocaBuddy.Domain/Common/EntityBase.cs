namespace VocaBuddy.Domain.Common;

public class EntityBase<TEntityId> : IAuditableEntity
{
    public TEntityId Id { get; init; } = default!;
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}