namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    [Inject]
    public IWordService? WordService { get; set; }

    protected int? MaxWordCount { set; get; }
    protected int WordCount { set; get; }
    protected GameMode GameMode { get; set; } = GameMode.Random;

    protected override async Task OnInitializedAsync()
    {
        var result = await WordService!.GetWordCountAsync();
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

    public void StartGame()
        => NavManager!.NavigateTo($"/gameplay?WordCount={WordCount}&GameMode={GameMode}");
}
