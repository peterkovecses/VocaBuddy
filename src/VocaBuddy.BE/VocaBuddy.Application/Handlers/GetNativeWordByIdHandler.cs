﻿using AutoMapper;
using MediatR;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;

namespace VocaBuddy.Application.Handlers;

public class GetNativeWordByIdHandler : IRequestHandler<GetNativeWordByIdQuery, NativeWordDto>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly IMapper _mapper;

    public GetNativeWordByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _mapper = mapper;
    }

    public async Task<NativeWordDto> Handle(GetNativeWordByIdQuery request, CancellationToken cancellationToken)
    {
        var nativeWord 
            = await _nativeWords.FindByIdAsync(request.WordId, cancellationToken) 
                ?? throw new NotFoundException(request.WordId);

        request.EntityUserId = nativeWord.UserId;

        return _mapper.Map<NativeWordDto>(nativeWord);
    }
}
