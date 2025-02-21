namespace VocaBuddy.Domain.Common;

public interface IAuditableEntity
{
    DateTime CreatedUtc { set; }
    DateTime UpdatedUtc { set; }
}