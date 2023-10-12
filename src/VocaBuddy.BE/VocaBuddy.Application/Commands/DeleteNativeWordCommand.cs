using MediatR;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Commands;

public record DeleteNativeWordCommand(int WordId) : IRequest<Result>;
