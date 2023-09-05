using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Commands;

public record CreateNativeWordCommand(NativeWordDto NativeWordDto) : IRequest<NativeWordDto>;
