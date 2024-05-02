namespace VocaBuddy.Application.Queries;

public record GetLatestNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
