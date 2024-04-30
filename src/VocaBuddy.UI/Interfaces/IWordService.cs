using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync();
    Task<List<CompactNativeWordDto>> GetRandomWordsAsync(int count);
    Task<List<CompactNativeWordDto>> GetLatestWordsAsync(int count);
    Task<Result<CompactNativeWordDto>> GetWordAsync(int id);
    Task<Result<int>> GetWordCountAsync();
    Task<Result> CreateWord(CompactNativeWordDto word);
    Task<Result> UpdateWord(CompactNativeWordDto word);
    Task<Result> DeleteWordAsync(int id);
}
