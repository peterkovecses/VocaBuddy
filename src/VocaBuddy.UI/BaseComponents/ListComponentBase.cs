namespace VocaBuddy.UI.BaseComponents;

public class ListComponentBase : ComponentBase
{
    protected bool Loading = true;
    protected string _filter = string.Empty;

    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 10;
    protected readonly List<int> PageSizes = new() { 5, 10, 25 };

    protected string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            CurrentPage = 1;
        }
    }

    protected void OnChangePage(int pageIndex)
        => CurrentPage = pageIndex;

    protected void OnChangePageSize(int size)
    {
        PageSize = size;
        CurrentPage = 1;
    }
}
