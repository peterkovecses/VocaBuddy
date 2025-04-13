namespace VocaBuddy.Application.Features.NativeWords.Queries.GetRandom;

public record GetRandomNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
