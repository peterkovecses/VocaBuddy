namespace VocaBuddy.Shared.Dtos;

public class NativeWordDto
{
    public int Id { get; set; }
    public required string Text { get; set; }
    public required string UserId { get; set; }
    public DateTime CreatedUtc { get; set; }

    public required virtual ICollection<ForeignWordDto> Translations { get; set; }
}
