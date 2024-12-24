namespace VocaBuddy.UI.Services;

public class GamePlayService(IWordService wordService) : IGamePlayService
{
    private List<CompactNativeWordDto>? _words;
    private List<CompactNativeWordDto> _mistakes = [];

    public CompactNativeWordDto? ActualWord { get; private set; }
    public int RemainingWordCount { get; private set; }
    public int? MistakeCount { get; private set; }
    public bool WordsNotLoaded => _words is null;

    public bool IsCorrectAnswer(string userInput)
    {
        var isCorrectAnswer =
            ActualWord!.Translations.Any(translation => string.Equals(translation.Text, userInput, StringComparison.CurrentCultureIgnoreCase));

        if (isCorrectAnswer)
        {
            RemainingWordCount--;
        }
        else
        {
            _mistakes.Add(ActualWord);
        }

        return isCorrectAnswer;
    }

    public async Task InitializeGame(GameMode gameMode, int wordCount)
    {
        await LoadWordsAsync();
        SetInitialRemainingWordCount();
        SetNextWord();
        
        return;

        async Task LoadWordsAsync()
        {
            _words = gameMode switch
            {
                GameMode.Random => await wordService.GetRandomWordsAsync(wordCount),
                GameMode.Latest => await wordService.GetLatestWordsAsync(wordCount),
                GameMode.Mistaken => await wordService.GetMistakenWordsAsync(wordCount),
                _ => throw new ArgumentException("Invalid game mode.", nameof(gameMode))
            };
        }

        void SetInitialRemainingWordCount()
            => RemainingWordCount = wordCount;
    }

    public void MarkActualWordAsAMistake()
        => _mistakes.Add(ActualWord!);

    public void SetNextWord()
    {
        ActualWord = _words!.First();
        _words!.Remove(ActualWord);
    }

    public void LoadMistakes()
    {
        _words = _mistakes;
        _mistakes = [];
    }

    public bool TryMoveToNextRound()
    {
        if (_words!.Count > 0)
        {
            SetNextWord();

            return true;
        }

        if (_mistakes.Count == 0) return false;
        var mistakenWordIds = GetMistakenWordIds();
        RecordMistakes(mistakenWordIds);
        SetInitialMistakeCount();
        LoadMistakes();
        SetNextWord();

        return true;
    }

    private List<int> GetMistakenWordIds() 
        => _mistakes.Select(word => word.Id).ToList();

    private void RecordMistakes(IEnumerable<int> mistakenWordIds)
        => Task.Run(() => wordService.RecordMistakes(mistakenWordIds));

    private void SetInitialMistakeCount()
        => MistakeCount ??= _mistakes.Count;
}
