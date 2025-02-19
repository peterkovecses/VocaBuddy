namespace VocaBuddy.UI.Services;

public class WordService(IVocaBuddyApiClient client) : IWordService
{
    public async Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync(CancellationToken cancellationToken)
    {
        var words = (await client.GetNativeWordsAsync(cancellationToken)).Data;

        return words.MapToListViewModels();
    }

    public async Task<List<CompactNativeWordDto>> GetRandomWordsAsync(int count, CancellationToken cancellationToken)
    {
        var result = await client.GetRandomNativeWordsAsync(count, cancellationToken);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public async Task<List<CompactNativeWordDto>> GetLatestWordsAsync(int count, CancellationToken cancellationToken)
    {
        var result = await client.GetLatestNativeWordsAsync(count, cancellationToken);
        ThrowIfFailure(result);

        return result.Data!;
    }

    public async Task<List<CompactNativeWordDto>> GetMistakenWordsAsync(int count, CancellationToken cancellationToken)
    {
        var result = await client.GetMistakenNativeWordsAsync(count, cancellationToken);
        ThrowIfFailure(result);

        return result.Data!;
    }
    
    public Task<Result<CompactNativeWordDto>> GetWordAsync(int id, CancellationToken cancellationToken)
        => client.GetNativeWordAsync(id, cancellationToken);

    public Task<Result<int>> GetWordCountAsync(CancellationToken cancellationToken)
        => client.GetNativeWordCountAsync(cancellationToken);

    public Task<Result> CreateWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken)
        => client.CreateNativeWordAsync(word, cancellationToken);

    public Task<Result> UpdateWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken)
        => client.UpdateNativeWordAsync(word, cancellationToken);

    public Task<Result> RecordMistakesAsync(IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken = default)
        => client.RecordMistakesAsync(mistakenWordIds, cancellationToken);
    
    public Task<Result> DeleteWordAsync(int id, CancellationToken cancellationToken)
        => client.DeleteNativeWordAsync(id, cancellationToken);

    private static void ThrowIfFailure<T>(Result<List<T>> result)
    {
        if (result.IsFailure)
        {
            throw new Exception("Failed to retrieve the words");
        }
    }
}
