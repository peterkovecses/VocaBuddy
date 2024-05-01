using AutoMapper;
using MediatR;

namespace VocaBuddy.Application.Handlers;

public class GetNativeWordByIdHandler : IRequestHandler<GetNativeWordByIdQuery, Result<CompactNativeWordDto?>>
{
    private readonly INativeWordRepository _nativeWords;
    private readonly IMapper _mapper;

    public GetNativeWordByIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _nativeWords = unitOfWork.NativeWords;
        _mapper = mapper;
    }

    public async Task<Result<CompactNativeWordDto?>> Handle(GetNativeWordByIdQuery request, CancellationToken cancellationToken)
    {
        var nativeWord
            = await _nativeWords.FindByIdAsync(request.WordId, cancellationToken);

        if (nativeWord is null)
        {
            return Result.Failure<CompactNativeWordDto?>(ErrorInfoFactory.NotFound(request.WordId));
        }

        request.EntityUserId = nativeWord.UserId;

        return Result.Success(_mapper.Map<CompactNativeWordDto?>(nativeWord));
    }
}
