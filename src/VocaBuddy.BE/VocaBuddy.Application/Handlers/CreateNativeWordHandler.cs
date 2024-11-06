namespace VocaBuddy.Application.Handlers;

public class CreateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper) : IRequestHandler<CreateNativeWordCommand, Result<NativeWordDto>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _currentUserId = user.Id!;
    private readonly IMapper _mapper = mapper;

    public async Task<Result<NativeWordDto>> Handle(CreateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWord = _mapper.Map<NativeWord>(request.NativeWorld);
        SetUserId();
        await SaveWordAsync();

        return Result.Success(_mapper.Map<NativeWordDto>(nativeWord));

        void SetUserId()
            => nativeWord.UserId = _currentUserId;

        async Task SaveWordAsync()
        {
            await _unitOfWork.NativeWords.AddAsync(nativeWord, cancellationToken);
            await _unitOfWork.CompleteAsync();
        }
    }
}
