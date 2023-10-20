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
                FailurePath();
            }

            MaxWordCount = result.Data;
        }
        catch
        {
            FailurePath();            
        }

    }

    public void StartGame()
        => NavManager.NavigateTo($"/gameplay/{WordCount}");

    private void FailurePath()
    {
        NotificationService.ShowFailure("Something went wrong, please try again later.");
        NavManager.NavigateTo("/");
    }
}
