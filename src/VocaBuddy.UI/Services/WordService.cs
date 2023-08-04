using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Services;

public class WordService : IWordService
{
    private readonly IVocaBuddyApiClient _client;

    public WordService(IVocaBuddyApiClient client)
    {
        _client = client;
    }

    public async Task<List<NativeWordViewModel>> GetWordsAsync()
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

    private static List<NativeWordViewModel> ConvertToWordsWithTranslations(List<NativeWordDto> words)
    {
        var result = new List<NativeWordViewModel>();

        foreach (var word in words)
        {
            result.Add(
                new NativeWordViewModel
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
