using System.Text;

namespace VocaBuddy.UI.Pages;

public class WordsBase : ComponentBase
{
    private string _filter = string.Empty;

    protected List<NativeWordViewModel> Words;
    protected bool Loading = true;

    [Inject]
    public IWordService WordService { get; set; }

    protected string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            CurrentPage = 1;
        }
    }

    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 10;
    protected int TotalItems { get; set; }
    protected readonly List<int> PageSizes = new() { 5, 10, 25 };

    protected List<NativeWordViewModel> FilteredWords
        => Words?.Where(word => ContainsTerm(word)).ToList() ?? new List<NativeWordViewModel>();

    protected List<NativeWordViewModel> PagedWords
        => FilteredWords.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

    protected async override Task OnInitializedAsync()
    {
        Loading = true;
        Words = await WordService.GetWordsAsync();
        Loading = false;
        TotalItems = Words.Count;
    }

    protected void ChangePage(int pageIndex)
        => CurrentPage = pageIndex;

    protected void ChangePageSize(int size)
    {
        PageSize = size;
        CurrentPage = 1;
    }

    private bool ContainsTerm(NativeWordViewModel word)
        => word.Text.Contains(
            Filter, StringComparison.OrdinalIgnoreCase)
                || word.TranslationsString.Contains(Filter, StringComparison.OrdinalIgnoreCase);
}
