using System.ComponentModel.DataAnnotations;
using VocaBuddy.Shared;
using VocaBuddy.UI.Validators;

namespace VocaBuddy.UI.Models;

public class NativeWordCreateUpdateModel
{
    public int Id { get; set; }

    [MaxLength(Constants.MaxWordLength, ErrorMessage = "The word cannot exceed {1} characters.")]
    public string Text { get; set; }

    [UniqueTranslations]
    public List<ForeignWordCreateUpdateModel> Translations { get; set; }
}
