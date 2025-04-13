namespace VocaBuddy.Application.Features.NativeWords.Queries.GetCount;
public record GetNativeWordCountQuery() : IRequest<Result<int>>;