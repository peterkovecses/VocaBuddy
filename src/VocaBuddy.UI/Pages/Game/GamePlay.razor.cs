using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Game;

public class GameplayBase : CustomComponentBase
{
    [Parameter]
    public int WordCount { set; get; }

    [Inject]
    public IWordService WordService { get; set; }

    protected List<NativeWordListViewModel> Words { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Words = await WordService.GetWordsAsync(WordCount);
    }
}
