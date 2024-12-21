namespace VocaBuddy.Domain.Entities;

public class ForeignWord : EntityBase
{
    public required string Text { get; set; }

    public required int NativeWordId { get; set; }
    public virtual NativeWord NativeWord { get; set; } = default!;
}
