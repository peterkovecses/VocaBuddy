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

    public Task<Result<NativeWordDto>> GetWordAsync(int id)
        => _client.GetNativeWordAsync(id);

    public Task<Result<int>> GetWordCountAsync()
        => _client.GetNativeWordCountAsync();

    public Task<Result> CreateWord(NativeWordDto word)
        => _client.CreateNativeWordAsync(word);

    public Task<Result> UpdateWord(NativeWordDto word)
        => _client.UpdateNativeWordAsync(word);

    public Task<Result> DeleteWordAsync(int id)
        => _client.DeleteNativeWordAsync(id);

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
                    CreatedUtc = word.CreatedUtc,
                    TranslationsString = string.Join(", ", word.Translations.Select(translation => translation.Text))
                });
        }

        return result;
    }
}
