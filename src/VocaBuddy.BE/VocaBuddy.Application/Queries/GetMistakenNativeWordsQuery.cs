namespace VocaBuddy.Application.Queries;

public record GetMistakenNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;