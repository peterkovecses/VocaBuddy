namespace VocaBuddy.Domain.Entities;

public class ForeignWord
{
    public int Id { get; set; }
    public required string Text { get; set; }

    public required int NativeWordId { get; set; }
    public virtual NativeWord NativeWord { get; set; }
}
