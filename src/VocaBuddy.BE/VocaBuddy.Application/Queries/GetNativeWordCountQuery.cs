namespace VocaBuddy.Application.Queries;
public record GetNativeWordCountQuery() : IRequest<Result<int>>;