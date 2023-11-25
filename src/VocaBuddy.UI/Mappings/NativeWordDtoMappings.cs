using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Mappings;

public static class NativeWordDtoMappings
{
    public static List<NativeWordListViewModel> MapToListViewModels(this List<NativeWordDto>? words)
    {
        ArgumentNullException.ThrowIfNull(words, nameof(words));
        var result = new List<NativeWordListViewModel>();

        foreach (var word in words)
        {
            result.Add(
                new NativeWordListViewModel
                {
                    Id = word.Id,
                    Text = word.Text,
                    CreatedUtc = word.CreatedUtc,
                    UpdatedUtc = word.UpdatedUtc,
                    TranslationsString = string.Join(", ", word.Translations.Select(translation => translation.Text))
                });
        }

        return result;
    }

    public static NativeWordCreateUpdateModel MapToCreateUpdateModel(this NativeWordDto? word)
    {
        ArgumentNullException.ThrowIfNull(word, nameof(word));

        return new NativeWordCreateUpdateModel
        {
            Id = word.Id,
            Text = word.Text,
            Translations = word.Translations
                    .Select(translation => new ForeignWordCreateUpdateModel { Id = translation.Id, Text = translation.Text })
                    .ToList()
        };
    }
}
