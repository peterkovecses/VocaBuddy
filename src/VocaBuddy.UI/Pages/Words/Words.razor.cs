using VocaBuddy.UI.BaseComponents;

namespace VocaBuddy.UI.Pages.Words;

public class WordsBase : ListComponentBase
{
    protected List<NativeWordListViewModel> Words;

    [Inject]
    public IWordService WordService { get; set; }

    protected async override Task OnInitializedAsync()
    {
        Loading = true;
        Words = await WordService.GetWordsAsync();
        Loading = false;
    }

    protected List<NativeWordListViewModel> FilteredWords
        => Words?.Where(word => ContainsTerm(word)).ToList() ?? new List<NativeWordListViewModel>();

    protected IEnumerable<NativeWordListViewModel> SortedWords
        => SortWords(FilteredWords);

    protected List<NativeWordListViewModel> PagedWords
        => SortedWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    private bool ContainsTerm(NativeWordListViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);

    private IOrderedEnumerable<NativeWordListViewModel> SortWords(ICollection<NativeWordListViewModel> words)
    {
        if (CurrentSortBy == SortBy.Alphabetical)
        {
            return CurrentSortOrder == SortOrder.Ascending
                ? words.OrderBy(w => w.Text)
                : words.OrderByDescending(w => w.Text);
        }
        else
        {
            return CurrentSortOrder == SortOrder.Ascending
                ? words.OrderBy(w => w.CreatedUtc)
                : words.OrderByDescending(w => w.CreatedUtc);
        }
    }
}