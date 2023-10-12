using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Commands;

public record UpdateNativeWordCommand(NativeWordDto NativeWordDto, int RouteId) : IRequest<Result>;
