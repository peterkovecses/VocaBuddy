using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    [Inject]
    public IWordService WordService { get; set; }

    protected int? MaxWordCount { set; get; }
    protected int WordCount { set; get; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var result = await WordService.GetWordCountAsync();
            if (result.IsFailure)
            {
                NotificationService.ShowFailure();
                NavManager.NavigateTo("/");
            }

            MaxWordCount = result.Data;
        }
        catch
        {
            NotificationService.ShowFailure();
            NavManager.NavigateTo("/");
        }

    }

    public void StartGame()
        => NavManager.NavigateTo($"/gameplay/{WordCount}");
}
