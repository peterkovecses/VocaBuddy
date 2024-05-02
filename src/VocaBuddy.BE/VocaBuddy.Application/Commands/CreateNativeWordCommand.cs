namespace VocaBuddy.Application.Commands;

public record CreateNativeWordCommand(CompactNativeWordDto NativeWorld) : IRequest<Result<NativeWordDto>>;
