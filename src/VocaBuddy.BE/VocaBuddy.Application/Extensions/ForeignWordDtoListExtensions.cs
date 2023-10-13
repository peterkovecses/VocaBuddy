using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Extensions;

public static class ForeignWordDtoListExtensions
{
    public static bool AllTranslationsAreUnique(this List<ForeignWordDto> translations)
        => translations
            .GroupBy(translation => translation.Text)
            .All(group => group.Count() == 1);
}
