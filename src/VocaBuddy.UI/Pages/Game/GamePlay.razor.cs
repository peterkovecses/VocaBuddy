﻿using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameplayBase : CustomComponentBase
{
    [Parameter]
    public int WordCount { set; get; }

    [Inject]
    public IWordService WordService { get; set; }

    protected List<NativeWordDto> Words { get; set; }
    protected NativeWordDto ActualWord { get; set; }
    protected List<NativeWordDto> Mistakes { get; set; } = new();
    protected string UserInput { get; set; } = string.Empty;
    protected bool IsSubmitted { get; set; }
    protected bool IsCorrectAnswer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ValidateWordCount();
        Words = await WordService.GetWordsAsync(WordCount);
        SetNextWord();
    }

    protected void OnSubmit()
    {
        IsSubmitted = true;
        IsCorrectAnswer = 
            ActualWord.Translations.Any(translation => translation.Text.ToLower() == UserInput.ToLower());

        if (!IsCorrectAnswer)
        {
            Mistakes.Add(ActualWord);
        }
    }

    protected void OnReveal()
    {
        IsSubmitted = true;
        IsCorrectAnswer = false;
        Mistakes.Add(ActualWord);
    }

    protected void NextRound()
    {
        if (Words.Any())
        {
            SetNextWord();
        }
        else if (Mistakes.Any())
        {
            LoadMistakes();
            SetNextWord();
        }
        else
        {
            EndGame();
        }

        ResetForm();
    }

    private void ValidateWordCount()
    {
        if (WordCount <= 0)
        {
            throw new Exception("The number of words must be greater than zero");
        }
    }

    private void SetNextWord()
    {
        ActualWord = Words.First();
        Words.Remove(ActualWord);
    }

    private void LoadMistakes()
    {
        Words = Mistakes;
        Mistakes = new();
    }

    private void ResetForm()
    {
        IsSubmitted = false;
        UserInput = string.Empty;
    }

    private void EndGame()
        => NavManager.NavigateTo("/game-results");
}
