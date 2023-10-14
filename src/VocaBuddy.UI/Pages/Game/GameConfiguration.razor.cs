using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    [Inject]
    public IWordService WordService { get; set; }

    protected int? MaxWordCount { set; get; }
    protected int WordCount { set; get; }

    protected override async Task OnInitializedAsync()
        => MaxWordCount = (await WordService.GetWordCountAsync()).Data;

    public void StartGame()
        => NavManager.NavigateTo($"/gameplay/{WordCount}");
}
