namespace VocaBuddy.Domain.Entities;

public class NativeWord
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedUtc { get; set; }

    public required virtual ICollection<ForeignWord> Translations { get; set; }
}
