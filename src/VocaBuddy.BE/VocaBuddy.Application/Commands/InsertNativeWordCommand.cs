using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Commands;

public record InsertNativeWordCommand(NativeWordDto NativeWordDto, string UserId) : IRequest<NativeWordDto>;
