namespace VocaBuddy.Application.Features.NativeWords.Queries.GetLatest;

public record GetLatestNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
