using MediatR;

namespace VocaBuddy.Application.Commands;

public record DeleteNativeWordCommand(int Id, string UserId) : IRequest;
