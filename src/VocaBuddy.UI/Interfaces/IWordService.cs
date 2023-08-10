using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordsAsync();
    Task<Result> CreateWord(NativeWordDto word);
    Task UpdateWord(NativeWordDto word);
}
