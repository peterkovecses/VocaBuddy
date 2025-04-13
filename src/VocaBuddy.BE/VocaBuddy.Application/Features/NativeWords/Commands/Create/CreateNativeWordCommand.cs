namespace VocaBuddy.Application.Features.NativeWords.Commands.Create;

public record CreateNativeWordCommand(CreateNativeWordDto NativeWord) : IRequest<Result<NativeWordDto>>;
