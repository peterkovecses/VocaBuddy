namespace VocaBuddy.Shared.Dtos;

public class NativeWordDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime CreatedUtc { get; set; }

    public virtual List<ForeignWordDto> Translations { get; set; }
}
