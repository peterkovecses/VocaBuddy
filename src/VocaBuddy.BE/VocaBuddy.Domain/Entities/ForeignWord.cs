namespace VocaBuddy.Domain.Entities;

public class ForeignWord : EntityBase<int>
{
    public required string Text { get; set; }

    public required int NativeWordId { get; set; }
    public NativeWord NativeWord { get; set; } = default!;
}
