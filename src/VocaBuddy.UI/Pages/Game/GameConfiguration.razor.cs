namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    [Inject]
    public IWordService? WordService { get; set; }

    protected int? MaxWordCount { set; get; }
    protected int WordCount { set; get; }
    protected string GameMode { get; set; } = GameModeConstants.Random;

    protected override async Task OnInitializedAsync()
    {
        var result = await WordService!.GetWordCountAsync(CancellationToken);
        if (result.IsFailure)
        {
            NavManager!.NavigateTo("/error");
        }

        MaxWordCount = result.Data;
    }

    protected void SetWordCount(ChangeEventArgs e)
    {
        WordCount = !string.IsNullOrEmpty((string)e.Value!) ? Convert.ToInt32(e.Value) : 0;
        StateHasChanged();
    }
    
    protected Action SetGameMode(string mode) 
        => () => GameMode = mode;

    protected void StartGame()
        => NavManager!.NavigateTo($"/gameplay?WordCount={WordCount}&GameMode={GameMode}");
}
