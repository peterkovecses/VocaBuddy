namespace VocaBuddy.Shared.Dtos;

public class CreateNativeWordDto : INativeWordDto
{
    public string Text { get; set; } = string.Empty;
    public List<ForeignWordDto> Translations { get; set; } = [];
}