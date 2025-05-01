namespace VocaBuddy.Shared.Dtos;

public class UpdateNativeWordDto
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<ForeignWordDto> Translations { get; set; } = [];
}