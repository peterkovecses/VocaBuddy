using MediatR;

namespace VocaBuddy.Application.Commands;

public record DeleteNativeWordCommand(int WordId) : IRequest<Unit>;
