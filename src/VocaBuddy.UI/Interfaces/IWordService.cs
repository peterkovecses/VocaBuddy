using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordWithTranslations>> GetWordsAsync();
    Task<NativeWordDto> GetWord(int id);
    Task UpdateWord(NativeWordDto word);
}
