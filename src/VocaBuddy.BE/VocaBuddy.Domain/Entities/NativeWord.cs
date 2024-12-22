namespace VocaBuddy.Domain.Entities;

public class NativeWord : EntityBase
{
    public required string Text { get; set; }
    public required string UserId { get; set; }
    public int MistakeCount { get; set; }

    public virtual required ICollection<ForeignWord> Translations { get; set; }
}
