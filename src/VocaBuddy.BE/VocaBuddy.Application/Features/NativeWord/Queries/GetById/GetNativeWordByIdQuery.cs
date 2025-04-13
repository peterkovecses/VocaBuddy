namespace VocaBuddy.Application.Features.NativeWord.Queries.GetById;

public record GetNativeWordByIdQuery(int WordId) : IRequest<Result<CompactNativeWordDto?>>
{
    public string? EntityUserId { get; set; }
}

