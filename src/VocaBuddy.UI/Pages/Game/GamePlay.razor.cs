using VocaBuddy.Shared.Dtos;
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
    protected bool CorrectAnswer { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ValidateWordCount();
        Words = await WordService.GetWordsAsync(WordCount);
        SetNextWord();
    }

    protected void EvaluateAnswer()
    {
        IsSubmitted = true;
        SetAnswerCorrectness();
    }

    protected void NextRound()
    {
        IsSubmitted = false;
        UserInput = string.Empty;

        if (CorrectAnswer)
        {
            if (Words.Any())
            {
                SetNextWord();
            }
            else if (Mistakes.Any())
            {
                ContinueWithMistakes();
                SetNextWord();
            }
            else
            {
                EndGame();
            }
        }
        else
        {
            Mistakes.Add(ActualWord);
            if (Words.Any())
            {
                SetNextWord();
            }
            else
            {
                ContinueWithMistakes();
                SetNextWord();
            }
        }
    }

    private void SetAnswerCorrectness()
    {
        if (ActualWord.Translations.Any(translation => translation.Text.ToLower() == UserInput.ToLower()))
        {
            CorrectAnswer = true;
        }
        else
        {
            CorrectAnswer = false;
        }
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

    private void ContinueWithMistakes()
    {
        Words = Mistakes;
        Mistakes = new();
    }

    private void EndGame()
        => NavManager.NavigateTo("/game-results");
}
