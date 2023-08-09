using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Services;

public class WordService : IWordService
{
    private readonly IVocaBuddyApiClient _client;

    public WordService(IVocaBuddyApiClient client)
    {
        _client = client;
    }

    public async Task<List<NativeWordListViewModel>> GetWordsAsync()
    {
        var words = (await _client.GetNativeWordsAsync()).Data;

        return ConvertToWordsWithTranslations(words);
    }

    public Task<NativeWordDto> GetWord(int id)
    {
        throw new NotImplementedException();
    }

    public Task CreateWord(NativeWordDto word)
    {
        throw new NotImplementedException();
    }

    public Task UpdateWord(NativeWordDto word)
    {
        throw new NotImplementedException();
    }

    private static List<NativeWordListViewModel> ConvertToWordsWithTranslations(List<NativeWordDto> words)
    {
        var result = new List<NativeWordListViewModel>();

        foreach (var word in words)
        {
            result.Add(
                new NativeWordListViewModel
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
