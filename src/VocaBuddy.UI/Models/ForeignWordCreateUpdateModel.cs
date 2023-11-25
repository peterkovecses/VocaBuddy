using System.ComponentModel.DataAnnotations;
using VocaBuddy.Shared;

namespace VocaBuddy.UI.Models;

public class ForeignWordCreateUpdateModel
{
    public int Id { get; set; }

    [MaxLength(Constants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; }
}
