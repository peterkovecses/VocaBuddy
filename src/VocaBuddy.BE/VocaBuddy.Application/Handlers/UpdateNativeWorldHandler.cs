using AutoMapper;
using MediatR;
using VocaBuddy.Application.Commands;
using VocaBuddy.Application.Exceptions;
using VocaBuddy.Application.Interfaces;

namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWorldHandler : IRequestHandler<UpdateNativeWordCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UpdateNativeWorldHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate 
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWordDto.Id, cancellationToken);

        if (nativeWordToUpdate == null) 
        {
            throw new NotFoundException(request.NativeWordDto.Id);
        }

        _mapper.Map(request.NativeWordDto, nativeWordToUpdate);
        await _unitOfWork.CompleteAsync();
    }
}
