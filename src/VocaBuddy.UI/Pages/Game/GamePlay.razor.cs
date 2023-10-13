using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameplayBase : CustomComponentBase
{
    [Parameter]
    public int WordCount { set; get; }
}
