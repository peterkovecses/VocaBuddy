namespace VocaBuddy.Shared.Dtos;

public class CompactNativeWordDto
{
    public int Id { get; set; }
    public string Text { get; set; } = default!;

    public List<CompactForeignWordDto> Translations { get; set; } = default!;
}
