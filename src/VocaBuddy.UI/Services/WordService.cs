namespace VocaBuddy.UI.Services;

public class WordService(IVocaBuddyApiClient client) : IWordService
{
    public async Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync()
    {
        var words = (await client.GetNativeWordsAsync()).Data;

        return words.MapToListViewModels();
    }

    public async Task<List<CompactNativeWordDto>> GetRandomWordsAsync(int count)
    {
        var result = await client.GetRandomNativeWordsAsync(count);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public async Task<List<CompactNativeWordDto>> GetLatestWordsAsync(int count)
    {
        var result = await client.GetLatestNativeWordsAsync(count);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public async Task<List<CompactNativeWordDto>> GetMistakenWordsAsync(int count)
    {
        var result = await client.GetMistakenNativeWordsAsync(count);
        ThrowIfFailure(result);

        return result.Data!;
    }
    
    public Task<Result<CompactNativeWordDto>> GetWordAsync(int id)
        => client.GetNativeWordAsync(id);

    public Task<Result<int>> GetWordCountAsync()
        => client.GetNativeWordCountAsync();

    public Task<Result> CreateWord(CompactNativeWordDto word)
        => client.CreateNativeWordAsync(word);

    public Task<Result> UpdateWord(CompactNativeWordDto word)
        => client.UpdateNativeWordAsync(word);

    public Task<Result> RecordMistakes(IEnumerable<int> mistakenWordIds)
        => client.RecordMistakesAsync(mistakenWordIds);
    
    public Task<Result> DeleteWordAsync(int id)
        => client.DeleteNativeWordAsync(id);

    private static void ThrowIfFailure<T>(Result<List<T>> result)
    {
        if (result.IsFailure)
        {
            throw new Exception("Failed to retrieve the words");
        }
    }
}
