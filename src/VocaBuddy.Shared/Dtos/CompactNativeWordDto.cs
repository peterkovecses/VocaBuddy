namespace VocaBuddy.Shared.Dtos;

public class CompactNativeWordDto
{
    public int Id { get; init; }
    public string Text { get; init; } = default!;
    public List<ForeignWordDto> Translations { get; init; } = default!;
}
