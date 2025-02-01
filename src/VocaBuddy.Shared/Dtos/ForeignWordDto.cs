namespace VocaBuddy.Shared.Dtos;

public class ForeignWordDto
{
    public int Id { get; set; }
    
    [MaxLength(ValidationConstants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; } = default!;
}
