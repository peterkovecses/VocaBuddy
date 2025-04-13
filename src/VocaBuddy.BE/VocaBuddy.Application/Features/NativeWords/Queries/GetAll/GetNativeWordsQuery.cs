namespace VocaBuddy.Application.Features.NativeWords.Queries.GetAll;

public record GetNativeWordsQuery : IRequest<Result<List<NativeWordDto>>>;
