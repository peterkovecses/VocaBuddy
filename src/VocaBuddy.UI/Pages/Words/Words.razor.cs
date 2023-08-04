namespace VocaBuddy.UI.Pages;

public class WordsBase : CustomComponentBase
{
    protected List<NativeWordViewModel> Words;
    protected bool Loading = true;

    [Inject]
    public IWordService WordService { get; set; }
    protected string Filter { get; set; } = string.Empty;
    protected List<NativeWordViewModel> FilteredWords
        => Words.Where(word => ContainsTerm(word)).ToList();

    protected override async Task OnInitializedAsync()
    {
        Loading = true;
        Words = await WordService.GetWordsAsync();
        Loading = false;
    }

    private bool ContainsTerm(NativeWordViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);
}
