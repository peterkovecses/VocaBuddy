using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordListViewModel>> GetWordsAsync();
    Task<NativeWordDto> GetWord(int id);
    Task CreateWord(NativeWordDto word);
    Task UpdateWord(NativeWordDto word);
}
