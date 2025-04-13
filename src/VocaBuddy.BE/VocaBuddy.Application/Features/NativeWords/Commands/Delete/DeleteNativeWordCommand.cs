namespace VocaBuddy.Application.Features.NativeWords.Commands.Delete;

public record DeleteNativeWordCommand(int WordId) : IRequest<Result>;
