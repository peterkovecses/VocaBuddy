using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameConfigurationBase : CustomComponentBase
{
    protected int WordCount { set; get; }

    public void StartGame()
        => NavManager.NavigateTo($"/gameplay/{WordCount}");
}
