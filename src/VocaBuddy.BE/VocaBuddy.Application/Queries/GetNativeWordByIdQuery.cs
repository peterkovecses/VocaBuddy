namespace VocaBuddy.Application.Queries;

public record GetNativeWordByIdQuery(int WordId) : IRequest<Result<CompactNativeWordDto?>>
{
    public string? EntityUserId { get; set; }
}

