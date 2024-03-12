using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.UI.Interfaces;

public interface IGamePlayService
{
    NativeWordDto? ActualWord { get; }
    int RemainingWordCount { get; }
    int? MistakeCount { get; }
    bool WordsNotLoaded { get; }

    bool IsCorrectAnswer(string userInput);
    Task InitializeGame(bool latestWords, int wordCount);
    void MarkActualWordAsAMistake();
    void SetNextWord();
    void LoadMistakes();
    bool TryMoveToNextRound();
}
