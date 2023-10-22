using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordDto>> GetWordsAsync(int? wordCount = default);
    Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync();
    Task<Result<NativeWordDto>> GetWordAsync(int id);
    Task<Result<int>> GetWordCountAsync();
    Task<Result> CreateWord(NativeWordDto word);
    Task<Result> UpdateWord(NativeWordDto word);
    Task<Result> DeleteWordAsync(int id);
}
