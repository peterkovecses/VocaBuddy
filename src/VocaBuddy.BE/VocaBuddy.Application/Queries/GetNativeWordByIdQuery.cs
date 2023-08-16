using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordByIdQuery(int Id, string UserId) : IRequest<NativeWordDto>;

