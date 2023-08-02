using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordsQuery(string UserId) : IRequest<List<NativeWordDto>>;
