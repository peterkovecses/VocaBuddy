namespace VocaBuddy.Application.Features.NativeWord.Queries.GetAll;

public record GetNativeWordsQuery : IRequest<Result<List<NativeWordDto>>>;
