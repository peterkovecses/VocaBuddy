namespace VocaBuddy.Application.Features.NativeWord.Commands.Create;

public record CreateNativeWordCommand(CreateNativeWordDto NativeWord) : IRequest<Result<NativeWordDto>>;
