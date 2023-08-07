namespace VocaBuddy.UI.Shared;

public class PaginationBase : ComponentBase
{
    [Parameter] 
    public int CurrentPage { get; set; }

    [Parameter] 
    public int PageSize { get; set; }

    [Parameter] 
    public int TotalItems { get; set; }

    public List<int> PageSizes { get; set; } = new() { 11, 20, 30 };

    [Parameter] 
    public EventCallback<int> OnPageChanged { get; set; }

    [Parameter] 
    public EventCallback<int> OnPageSizeChanged { get; set; }

    protected void OnPageSizeChangedHandler(ChangeEventArgs e)
    {
        PageSize = Convert.ToInt32(e.Value);
        OnPageSizeChanged.InvokeAsync(PageSize);
    }
}
