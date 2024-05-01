namespace VocaBuddy.UI.Mappings;

public static class NativeWordDtoMappings
{
    public static List<NativeWordListViewModel> MapToListViewModels(this List<NativeWordDto>? words)
    {
        ArgumentNullException.ThrowIfNull(words, nameof(words));

        return words.Select(word => new NativeWordListViewModel
            {
                Id = word.Id,
                Text = word.Text,
                CreatedUtc = word.CreatedUtc,
                UpdatedUtc = word.UpdatedUtc,
                TranslationsString = string.Join(", ", word.Translations.Select(translation => translation.Text))
            })
            .ToList();
    }
}
