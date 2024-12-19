namespace VocaBuddy.Application.Commands;

public record CreateNativeWordCommand(CompactNativeWordDto NativeWord) : IRequest<Result<NativeWordDto>>;
