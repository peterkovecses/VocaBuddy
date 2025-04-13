namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync(CancellationToken cancellationToken);
    Task<List<CompactNativeWordDto>> GetRandomWordsAsync(int count, CancellationToken cancellationToken);
    Task<List<CompactNativeWordDto>> GetLatestWordsAsync(int count, CancellationToken cancellationToken);
    Task<List<CompactNativeWordDto>> GetMistakenWordsAsync(int count, CancellationToken cancellationToken);
    Task<Result<CompactNativeWordDto>> GetWordAsync(int id, CancellationToken cancellationToken);
    Task<Result<int>> GetWordCountAsync(CancellationToken cancellationToken);
    Task<Result> CreateWordAsync(CreateNativeWordDto word, CancellationToken cancellationToken);
    Task<Result> UpdateWordAsync(UpdateNativeWordDto word, CancellationToken cancellationToken);
    Task<Result> RecordMistakesAsync(IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken = default);
    Task<Result> DeleteWordAsync(int id, CancellationToken cancellationToken);
}
