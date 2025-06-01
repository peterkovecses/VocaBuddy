namespace VocaBuddy.Application.Features.NativeWord.Queries.GetLatest;

public record GetLatestNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
