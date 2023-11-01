using MediatR;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Queries;

public record GetRandomNativeWordsQuery(int WordCount) : IRequest<Result<List<NativeWordDto>>>;
