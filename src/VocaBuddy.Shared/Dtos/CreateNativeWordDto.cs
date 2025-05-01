using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.Shared.Dtos;

public class CreateNativeWordDto : INativeWord
{
    public string Text { get; set; } = string.Empty;
    public List<ForeignWordDto> Translations { get; set; } = [];
}