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

    protected List<NativeWordListViewModel> SortedWords
        => (CurrentSortOrder == SortOrder.Ascending
                ? FilteredWords.OrderBy(word => CurrentSortBy == SortBy.Alphabetical ? word.Text : word.CreatedUtc.ToString())
                : FilteredWords.OrderByDescending(word => CurrentSortBy == SortBy.Alphabetical ? word.Text : word.CreatedUtc.ToString()))
            .ToList();

    protected List<NativeWordListViewModel> PagedWords
        => SortedWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    private bool ContainsTerm(NativeWordListViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);
}