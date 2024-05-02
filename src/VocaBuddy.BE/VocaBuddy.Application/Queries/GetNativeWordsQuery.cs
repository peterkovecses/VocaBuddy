namespace VocaBuddy.Application.Queries;

public record GetNativeWordsQuery() : IRequest<Result<List<NativeWordDto>>>;
