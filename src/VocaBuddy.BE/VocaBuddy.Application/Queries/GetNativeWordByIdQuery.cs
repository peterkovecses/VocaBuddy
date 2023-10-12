using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordByIdQuery(int WordId) : IRequest<Result<NativeWordDto?>>
{
    public string? EntityUserId { get; set; }
}

