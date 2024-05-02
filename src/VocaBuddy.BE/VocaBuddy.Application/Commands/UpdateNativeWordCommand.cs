namespace VocaBuddy.Application.Commands;

public record UpdateNativeWordCommand(CompactNativeWordDto NativeWord, int RouteId) : IRequest<Result>;