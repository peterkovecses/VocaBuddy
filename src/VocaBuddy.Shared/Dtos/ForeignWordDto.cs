namespace VocaBuddy.Shared.Dtos;

public class ForeignWordDto
{
    public int Id { get; set; }
    public required string Text { get; set; }

    public required int NativeWordId { get; set; }
}
