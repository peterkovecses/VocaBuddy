using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Extensions;

public static class NativeWordDtoExtensions
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
}
