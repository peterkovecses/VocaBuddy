using System.ComponentModel.DataAnnotations;

namespace VocaBuddy.UI.Validators;

public class UniqueTranslationsAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not List<ForeignWordCreateUpdateModel> translations)
        {
            return true;
        }

        var translationsData = translations.Select(word => word.Text.ToLowerInvariant()).ToList();

        return translationsData.Count == translationsData.Distinct().Count();
    }

    public override string FormatErrorMessage(string name)
    {
        return "All translations must be unique.";
    }
}

