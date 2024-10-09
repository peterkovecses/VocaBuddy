namespace VocaBuddy.UI.Services;

public class GamePlayService(IWordService wordService) : IGamePlayService
{
    private readonly IWordService _wordService = wordService;
    private List<CompactNativeWordDto>? _words;
    private List<CompactNativeWordDto> _mistakes = new();

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

    public async Task InitializeGame(bool latestWords, int wordCount)
    {
        await LoadWordsAsync();
        SetInitialRemainingWordCount();
        SetNextWord();
        
        return;

        async Task LoadWordsAsync()
        {
            if (latestWords)
            {
                _words = await _wordService!.GetLatestWordsAsync(wordCount);
            }
            else
            {
                _words = await _wordService!.GetRandomWordsAsync(wordCount);
            }
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
        if (_words!.Any())
        {
            SetNextWord();

            return true;
        }

        if (!_mistakes!.Any()) return false;
        SetInitialMistakeCount();
        LoadMistakes();
        SetNextWord();

        return true;

    }

    private void SetInitialMistakeCount()
        => MistakeCount ??= _mistakes.Count;
}
