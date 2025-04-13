namespace VocaBuddy.Application.Features.NativeWord.Queries.GetCount;
public record GetNativeWordCountQuery() : IRequest<Result<int>>;