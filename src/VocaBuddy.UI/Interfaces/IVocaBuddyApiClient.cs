namespace VocaBuddy.UI.Interfaces;

public interface IVocaBuddyApiClient
{
    Task<Result<List<NativeWordDto>>> GetNativeWordsAsync(CancellationToken cancellationToken);
    Task<Result<List<CompactNativeWordDto>>> GetRandomNativeWordsAsync(int count, CancellationToken cancellationToken);
    Task<Result<List<CompactNativeWordDto>>> GetLatestNativeWordsAsync(int count, CancellationToken cancellationToken);
    Task<Result<List<CompactNativeWordDto>>> GetMistakenNativeWordsAsync(int count, CancellationToken cancellationToken);
    Task<Result<CompactNativeWordDto>> GetNativeWordAsync(int id, CancellationToken cancellationToken);
    Task<Result<int>> GetNativeWordCountAsync(CancellationToken cancellationToken);
    Task<Result> CreateNativeWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken);
    Task<Result> UpdateNativeWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken);
    Task<Result> RecordMistakesAsync(IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken);
    Task<Result> DeleteNativeWordAsync(int id, CancellationToken cancellationToken);
}
