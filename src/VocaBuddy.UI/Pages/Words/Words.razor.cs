using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class WordsBase : ListComponentBase
{
    protected List<NativeWordViewModel> Words;

    [Inject]
    public IWordService WordService { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Loading = true;
        Words = await WordService.GetWordsAsync();
        Loading = false;
    }

    protected List<NativeWordViewModel> FilteredWords
        => Words?.Where(word => ContainsTerm(word)).ToList() ?? new List<NativeWordViewModel>();

    protected List<NativeWordViewModel> PagedWords
        => FilteredWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    private bool ContainsTerm(NativeWordViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);
}