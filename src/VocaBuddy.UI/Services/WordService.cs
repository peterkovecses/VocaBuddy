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

    public async Task<Result> CreateWord(NativeWordDto word)
        => await _client.CreateNativeWord(word);

    public async Task<Result> UpdateWord(NativeWordDto word)
        => await _client.UpdateNativeWord(word);

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
