using AutoMapper;
using MediatR;
using System.Linq.Expressions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Domain.Entities;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Handlers;

public class GetNativeWordsHandler : IRequestHandler<GetNativeWordsQuery, List<NativeWordDto>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly string _currentUserId;
    private readonly IMapper _mapper;

    public GetNativeWordsHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _currentUserId = user.Id!;
        _mapper = mapper;
    }

    public async Task<List<NativeWordDto>> Handle(
        GetNativeWordsQuery request,
        CancellationToken cancellationToken)
    {
        Expression<Func<NativeWord, bool>> predicate = word => word.UserId == _currentUserId; 
        var nativeWords = await _nativeWords.GetAsync(predicate, cancellationToken, request.ItemCount);

        return _mapper.Map<List<NativeWordDto>>(nativeWords);
    }
}
