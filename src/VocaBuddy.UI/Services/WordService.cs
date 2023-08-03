using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Services;

public class WordService : IWordService
{
    private readonly IVocaBuddyApiClient _client;

    public WordService(IVocaBuddyApiClient client)
    {
        _client = client;
    }

    public async Task<List<NativeWordWithTranslations>> GetWordsAsync()
    {
        var words = (await _client.GetNativeWordsAsync()).Data;

        return ConvertToWordsWithTranslations(words);
    }

    public Task<NativeWordDto> GetWord(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateWord(NativeWordDto word)
    {
        throw new NotImplementedException();
    }

    private static List<NativeWordWithTranslations> ConvertToWordsWithTranslations(List<NativeWordDto> words)
    {
        var result = new List<NativeWordWithTranslations>();

        foreach (var word in words)
        {
            result.Add(
                new NativeWordWithTranslations
                {
                    Id = word.Id,
                    Text = word.Text,
                    UserId = word.UserId,
                    CreatedUtc = word.CreatedUtc,
                    TranslationsString = string.Join(", ", word.Translations.Select(translation => translation.Text))
                });
        }

        return result;
    }
}
