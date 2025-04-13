namespace VocaBuddy.Shared.Dtos;

public class NativeWordDto
{
    public int Id { get; init; }
    public string Text { get; init; } = default!;
    public DateTime CreatedUtc { get; init; }
    public DateTime UpdatedUtc { get; init; }

    public List<ForeignWordDto> Translations { get; init; } = default!;
}
