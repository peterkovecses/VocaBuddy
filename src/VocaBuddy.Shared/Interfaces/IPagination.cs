namespace VocaBuddy.Shared.Interfaces;

public interface IPagination
{
    int PageNumber { get; }
    int PageSize { get; }
    public bool IsNoPagination { get; }
}
