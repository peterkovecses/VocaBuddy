namespace VocaBuddy.Domain.Common;

public class EntityBase
{
    public int Id { get; set; }
    public DateTime CreatedUtc { get; set; }
    public DateTime UpdatedUtc { get; set; }
}