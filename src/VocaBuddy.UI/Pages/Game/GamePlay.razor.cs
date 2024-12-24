namespace VocaBuddy.UI.Pages.Game;

public class GamePlayBase : CustomComponentBase
{
    [Parameter]
    [SupplyParameterFromQuery]
    public int WordCount { set; get; }

    [Parameter]
    [SupplyParameterFromQuery]
    public GameMode GameMode { set; get; } = GameMode.Random;

    protected string UserInput { set; get; } = string.Empty;

    [Inject] 
    protected IGamePlayService? GamePlayService { get; set; }

    protected bool IsSubmitted { get; set; }
    protected bool IsCorrectAnswer { get; set; }
    protected bool IsRevealed { get; set; }

    protected override async Task OnInitializedAsync()
    {
        ValidateWordCount();
        await GamePlayService!.InitializeGame(GameMode, WordCount);
    }

    protected void OnSubmit()
    {
        IsSubmitted = true;
        IsCorrectAnswer = GamePlayService!.IsCorrectAnswer(UserInput);
    }

    protected void OnReveal()
    {
        IsSubmitted = true;
        IsRevealed = true;
        IsCorrectAnswer = false;
        GamePlayService!.MarkActualWordAsAMistake();
    }

    protected void NextRound()
    {
        if (GamePlayService!.TryMoveToNextRound())
        {
            ResetForm();
        }
        else
        {
            EndGame();
        }
    }

    private void ValidateWordCount()
    {
        if (WordCount <= 0)
        {
            throw new Exception("The number of words must be greater than zero");
        }
    }

    private void ResetForm()
    {
        IsSubmitted = false;
        IsRevealed = false;
        UserInput = string.Empty;
    }

    private void EndGame()
        => NavManager!.NavigateTo($"/game-results/{GamePlayService!.MistakeCount}");
}
