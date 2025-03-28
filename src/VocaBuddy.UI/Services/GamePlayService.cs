﻿namespace VocaBuddy.UI.Services;

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

    public async Task InitializeGameAsync(string gameMode, int wordCount, CancellationToken cancellationToken)
    {
        await LoadWordsAsync();
        SetInitialRemainingWordCount();
        SetNextWord();
        
        return;

        async Task LoadWordsAsync()
        {
            _words = gameMode switch
            {
                GameModeConstants.Random => await wordService.GetRandomWordsAsync(wordCount, cancellationToken),
                GameModeConstants.Latest => await wordService.GetLatestWordsAsync(wordCount, cancellationToken),
                GameModeConstants.Mistaken => await wordService.GetMistakenWordsAsync(wordCount, cancellationToken),
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
        => _ = wordService.RecordMistakesAsync(mistakenWordIds);

    private void SetInitialMistakeCount()
        => MistakeCount ??= _mistakes.Count;
}
