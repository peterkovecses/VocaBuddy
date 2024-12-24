namespace VocaBuddy.UI.Interfaces;

public interface IGamePlayService
{
    CompactNativeWordDto? ActualWord { get; }
    int RemainingWordCount { get; }
    int? MistakeCount { get; }
    bool WordsNotLoaded { get; }

    bool IsCorrectAnswer(string userInput);
    Task InitializeGame(GameMode gameMode, int wordCount);
    void MarkActualWordAsAMistake();
    void SetNextWord();
    void LoadMistakes();
    bool TryMoveToNextRound();
}
