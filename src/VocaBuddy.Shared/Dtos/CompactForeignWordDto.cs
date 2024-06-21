using System.ComponentModel.DataAnnotations;
using VocaBuddy.Shared.Constants;

namespace VocaBuddy.Shared.Dtos;

public class CompactForeignWordDto
{
    public int Id { get; set; }

    [MaxLength(ValidationConstants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; } = string.Empty;
}
