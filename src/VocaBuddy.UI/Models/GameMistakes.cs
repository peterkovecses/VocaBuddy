namespace VocaBuddy.UI.Models;

public class GameMistakes
{
    private readonly List<WordMistake> _wordMistakes = [];
    
    public IReadOnlyList<WordMistake> WordMistakes => _wordMistakes;

    public void AddMistake(int nativeWordId, int mistakeCount)
    {
        var existingMistake = _wordMistakes.FirstOrDefault(mistake => mistake.NativeWordId == nativeWordId);
        if (existingMistake is not null)
        {
            existingMistake.MistakeCount++;
        }
        else
        {
            _wordMistakes.Add(new WordMistake { NativeWordId = nativeWordId, MistakeCount = mistakeCount });
        }
    }
}