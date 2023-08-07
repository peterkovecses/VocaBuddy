namespace VocaBuddy.UI.Shared;

public class PaginationBase : ComponentBase
{
    [Parameter] 
    public int CurrentPage { get; set; }

    [Parameter]
    public List<int> PageSizes { get; set; } = new List<int> { 15, 25, 50 };

    [Parameter] 
    public int PageSize { get; set; }

    [Parameter] 
    public int TotalItems { get; set; }

    [Parameter] 
    public EventCallback<int> OnPageChanged { get; set; }

    [Parameter] 
    public EventCallback<int> OnPageSizeChanged { get; set; }

    protected int TotalPages => (int)Math.Ceiling(TotalItems / (Decimal)PageSize);
    protected int DisplayedPages => Math.Min(TotalPages, 5);
    protected int FirstDisplayedPage => Math.Max(1, Math.Min(TotalPages - DisplayedPages + 1, CurrentPage - DisplayedPages / 2));
    protected int LastDisplayedPage => FirstDisplayedPage + DisplayedPages - 1;

    protected void OnPageSizeChangedHandler(ChangeEventArgs e)
    {
        PageSize = Convert.ToInt32(e.Value);
        OnPageSizeChanged.InvokeAsync(PageSize);
    }

    protected void NavigateToPreviousPage()
    {
        if (CurrentPage > 1)
        {
            OnPageChanged.InvokeAsync(CurrentPage - 1);
        }
    }

    protected void NavigateToNextPage()
    {
        if (CurrentPage < TotalPages)
        {
            OnPageChanged.InvokeAsync(CurrentPage + 1);
        }
    }

    protected string DisableWhenCurrentIsFirstPage()
        => CurrentPage == 1 ? "disabled" : string.Empty;

    protected string DisableWhenCurrentIsLastPage()
        => CurrentPage >= TotalPages ? "disabled" : string.Empty;

    protected bool RequiresDisplayFirstPageSeparately()
        => FirstDisplayedPage != 1;

    protected bool RequiresDisplayLastPageSeparately()
        => LastDisplayedPage != TotalPages;

    protected bool RequiresGapBeforeFirstDisplayedPage()
        => FirstDisplayedPage > 2;

    protected bool RequiresGapAfterLastDisplayedPage()
        => LastDisplayedPage < TotalPages - 1;

    protected string IsFirstPage()
        => CurrentPage == 1 ? "true" : "false";

    protected string IsLastPage()
        => CurrentPage >= TotalPages ? "true" : "false";

    protected string EnableWhenCurrentPageIsFirstPage()
        => CurrentPage == 1 ? "active" : string.Empty;

    protected string EnableWhenPageNumberEqualsCurrentPage(int pageNumber)
        => CurrentPage == pageNumber ? "active" : string.Empty;

    protected string EnableWhenCurrentPageIsLastPage()
        => TotalPages == CurrentPage ? "active" : string.Empty;
}
