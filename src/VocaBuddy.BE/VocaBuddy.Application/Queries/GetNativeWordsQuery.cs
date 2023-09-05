using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordsQuery() : IRequest<List<NativeWordDto>>;
