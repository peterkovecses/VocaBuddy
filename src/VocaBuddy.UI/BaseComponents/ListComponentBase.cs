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

    protected SortOrder CurrentSortOrder { get; set; } = SortOrder.Ascending;
    protected SortType CurrentSortType { get; set; } = SortType.Alphabetical;

    protected void OnChangePage(int pageIndex)
        => CurrentPage = pageIndex;

    protected void OnChangePageSize(int size)
    {
        PageSize = size;
        CurrentPage = 1;
    }

    protected void ChangeSortType(string sortType)
    {
        if (Enum.TryParse<SortType>(sortType, out var result))
        {
            SortTable(result);
        }
    }

    protected void ChangeSortOrder(ChangeEventArgs e)
    {
        bool isDescending = Convert.ToBoolean(e.Value);
        CurrentSortOrder = isDescending ? SortOrder.Ascending : SortOrder.Descending;
        SortTable(CurrentSortType);
    }

    protected void SortTable(SortType sortType)
    {
        if (CurrentSortType != sortType)
        {
            CurrentSortOrder = SortOrder.Ascending;
        }
        else
        {
            CurrentSortOrder = CurrentSortOrder == SortOrder.Ascending
                ? SortOrder.Descending
                : SortOrder.Ascending;
        }

        CurrentSortType = sortType;
    }

    protected void ToggleSortOrder()
    {
        CurrentSortOrder = CurrentSortOrder == SortOrder.Ascending
            ? SortOrder.Descending
            : SortOrder.Ascending;
    }

    protected enum SortOrder
    {
        Ascending,
        Descending
    }

    protected enum SortType
    {
        Alphabetical,
        CreatedUtc
    }
}
