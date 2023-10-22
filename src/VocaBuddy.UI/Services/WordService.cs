using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.Services;

public class WordService : IWordService
{
    private readonly IVocaBuddyApiClient _client;

    public WordService(IVocaBuddyApiClient client)
    {
        _client = client;
    }

    public async Task<List<NativeWordDto>> GetWordsAsync(int? wordCount = default)
    {
        var result = await _client.GetNativeWordsAsync(wordCount);
        if (result.IsFailure)
        {
            throw new Exception("Failed to retrieve the words");
        }

        return result.Data!;
    }

    public async Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync()
    {
        var words = await GetWordsAsync();

        return words.MapToListViewModels();
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
}
