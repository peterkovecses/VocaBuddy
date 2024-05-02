namespace VocaBuddy.Application.Commands;

public record DeleteNativeWordCommand(int WordId) : IRequest<Result>;
