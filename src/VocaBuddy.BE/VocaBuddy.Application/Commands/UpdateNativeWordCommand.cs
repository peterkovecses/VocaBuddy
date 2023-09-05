﻿using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Commands;

public record UpdateNativeWordCommand(NativeWordDto NativeWordDto, int RouteId, string UserId) : IRequest<Unit>;
