namespace VocaBuddy.Application.Features.NativeWords.Queries.GetMistaken;

public record GetMistakenNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;