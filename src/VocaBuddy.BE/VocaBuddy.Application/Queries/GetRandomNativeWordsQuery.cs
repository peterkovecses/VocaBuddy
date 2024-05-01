using MediatR;

namespace VocaBuddy.Application.Queries;

public record GetRandomNativeWordsQuery(int WordCount) : IRequest<Result<List<CompactNativeWordDto>>>;
