using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Commands;

public record UpdateNativeWordCommand(CompactNativeWordDto NativeWord, int RouteId) : IRequest<Result>;
