namespace VocaBuddy.Application.Features.NativeWord.Commands.Update;

public record UpdateNativeWordCommand(UpdateNativeWordDto NativeWord, int RouteId) : IRequest<Result>;