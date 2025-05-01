using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.Shared.Dtos;

public class UpdateNativeWordDto : INativeWord
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<ForeignWordDto> Translations { get; set; } = [];
}