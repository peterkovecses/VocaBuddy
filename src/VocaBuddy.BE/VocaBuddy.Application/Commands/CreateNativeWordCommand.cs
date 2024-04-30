using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Commands;

public record CreateNativeWordCommand(CompactNativeWordDto NativeWorld) : IRequest<Result<NativeWordDto>>;
