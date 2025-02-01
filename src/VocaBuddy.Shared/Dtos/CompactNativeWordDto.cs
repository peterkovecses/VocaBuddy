namespace VocaBuddy.Shared.Dtos;

public class CompactNativeWordDto
{
    public int Id { get; set; }
    
    [MaxLength(ValidationConstants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; } = default!;

    public List<ForeignWordDto> Translations { get; set; } = default!;
}
