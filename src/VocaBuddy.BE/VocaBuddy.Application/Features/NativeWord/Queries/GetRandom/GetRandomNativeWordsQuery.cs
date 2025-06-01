namespace VocaBuddy.Application.Features.NativeWord.Queries.GetRandom;

public record GetRandomNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
