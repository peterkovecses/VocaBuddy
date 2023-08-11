namespace VocaBuddy.UI.BaseComponents;

public class ListComponentBase : CustomComponentBase
{
    private string _filter = string.Empty;
    protected bool ConfirmDeleteVisible;
    protected int ItemToDeleteId;

    protected int CurrentPage { get; set; } = 1;
    protected int PageSize { get; set; } = 15;
    protected SortOrder CurrentSortOrder { get; set; } = SortOrder.Ascending;
    protected SortBy CurrentSortBy { get; set; } = SortBy.Alphabetical;

    protected string Filter
    {
        get => _filter;
        set
        {
            _filter = value;
            CurrentPage = 1;
        }
    }

    protected void ShowConfirmDelete() => ConfirmDeleteVisible = true;

    protected void CloseConfirmDelete() => ConfirmDeleteVisible = false;

    protected void ConfirmDeleteWordAsync(int id)
    {
        ItemToDeleteId = id;
        ShowConfirmDelete();
    }

    protected void OnChangePage(int pageIndex)
        => CurrentPage = pageIndex;

    protected void OnChangePageSize(int size)
    {
        PageSize = size;
        CurrentPage = 1;
    }

    protected void SetSortBy(string sortType)
    {
        if (Enum.TryParse<SortBy>(sortType, out var result))
        {
            CurrentSortBy = result;
        }
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

    protected enum SortBy
    {
        Alphabetical,
        CreatedUtc
    }
}
