using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.Shared.Dtos;

public class ForeignWordCreateUpdateModel
{
    public int Id { get; set; }

    [MaxLength(Constants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; } = string.Empty;
}
