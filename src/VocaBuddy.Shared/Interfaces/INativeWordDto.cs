using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Shared.Interfaces;

public interface INativeWordDto
{
    string Text { get; set; }
    List<ForeignWordDto> Translations { get; set; }
}