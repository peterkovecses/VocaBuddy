using AutoMapper;
using MediatR;
using VocaBuddy.Application.Errors;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Application.Handlers;

public class GetNativeWordByIdHandler : IRequestHandler<GetNativeWordByIdQuery, Result<NativeWordDto?>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly IMapper _mapper;

    public GetNativeWordByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _mapper = mapper;
    }

    public async Task<Result<NativeWordDto?>> Handle(GetNativeWordByIdQuery request, CancellationToken cancellationToken)
    {
        var nativeWord
            = await _nativeWords.FindByIdAsync(request.WordId, cancellationToken);

        if (nativeWord is null)
        {
            return Result.Failure<NativeWordDto?>(ErrorInfoFactory.NotFound(request.WordId));
        }

        request.EntityUserId = nativeWord.UserId;

        return Result.Success(_mapper.Map<NativeWordDto?>(nativeWord));
    }
}
