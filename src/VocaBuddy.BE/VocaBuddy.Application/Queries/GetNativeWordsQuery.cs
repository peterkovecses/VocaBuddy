using MediatR;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Queries;

public record GetNativeWordsQuery(int? RandomItemCount) : IRequest<List<NativeWordDto>>;
