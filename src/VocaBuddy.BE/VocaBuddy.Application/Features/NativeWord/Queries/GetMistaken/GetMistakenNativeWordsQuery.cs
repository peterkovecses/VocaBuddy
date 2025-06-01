namespace VocaBuddy.Application.Features.NativeWord.Queries.GetMistaken;

public record GetMistakenNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;