using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;

namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWordHandler : IRequestHandler<UpdateNativeWordCommand, Unit>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateNativeWordHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate 
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWordDto.Id, cancellationToken) 
                ?? throw new NotFoundException(request.NativeWordDto.Id);

        if (nativeWordToUpdate.UserId != request.UserId)
        {
            throw new UserIdNotMatchException();
        }

        _mapper.Map(request.NativeWordDto, nativeWordToUpdate);
        await _unitOfWork.CompleteAsync();

        return Unit.Value;
    }
}
