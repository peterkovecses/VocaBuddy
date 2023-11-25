using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordListViewModelsAsync();
    Task<List<NativeWordDto>> GetRandomWordsAsync(int count);
    Task<List<NativeWordDto>> GetLatestWordsAsync(int count);
    Task<Result<NativeWordDto>> GetWordAsync(int id);
    Task<Result<int>> GetWordCountAsync();
    Task<Result> CreateWord(NativeWordCreateUpdateModel word);
    Task<Result> UpdateWord(NativeWordCreateUpdateModel word);
    Task<Result> DeleteWordAsync(int id);
}
