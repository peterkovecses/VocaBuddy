namespace VocaBuddy.UI.Interfaces;

public interface IWordService
{
    Task<List<NativeWordWithTranslations>> GetWordsAsync(); 
}
