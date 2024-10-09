namespace VocaBuddy.Application.Handlers;

public class UpdateNativeWordHandler(IUnitOfWork unitOfWork, ICurrentUser user, IMapper mapper) : IRequestHandler<UpdateNativeWordCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _currentUserId = user.Id!;
    private readonly IMapper _mapper = mapper;

    public async Task<Result> Handle(UpdateNativeWordCommand request, CancellationToken cancellationToken)
    {
        var nativeWordToUpdate
            = await _unitOfWork.NativeWords.FindByIdAsync(request.NativeWord.Id, cancellationToken);

        if (nativeWordToUpdate is null)
        {
            return Result.Failure(ErrorInfoFactory.NotFound(request.NativeWord.Id));
        }

        if (!ValidateUserId(nativeWordToUpdate))
        {
            return Result.Failure(ErrorInfoFactory.UserIdNotMatch());
        }

        await UpdateWordAsync();

        return Result.Success();

        bool ValidateUserId(NativeWord nativeWordToDelete)
            => nativeWordToDelete.UserId == _currentUserId;

        async Task UpdateWordAsync()
        {
            _mapper.Map(request.NativeWord, nativeWordToUpdate);
            await _unitOfWork.CompleteAsync();
        }
    }
}
