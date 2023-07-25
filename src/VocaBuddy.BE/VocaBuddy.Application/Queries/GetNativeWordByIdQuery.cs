using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordByIdQuery(int Id) : IRequest<NativeWordDto>;

