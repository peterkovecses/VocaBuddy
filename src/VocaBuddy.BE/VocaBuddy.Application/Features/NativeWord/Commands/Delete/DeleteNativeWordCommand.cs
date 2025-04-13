namespace VocaBuddy.Application.Features.NativeWord.Commands.Delete;

public record DeleteNativeWordCommand(int WordId) : IRequest<Result>;
