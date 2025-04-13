namespace VocaBuddy.Application.Features.NativeWords.Commands.Update;

public record UpdateNativeWordCommand(UpdateNativeWordDto NativeWord, int RouteId) : IRequest<Result>;