namespace VocaBuddy.Application.Handlers;

public class GetNativeWordCountHandler : IRequestHandler<GetNativeWordCountQuery, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUser _currentUser;

    public GetNativeWordCountHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    {
        _unitOfWork = unitOfWork;
        _currentUser = currentUser;
    }

    public async Task<Result<int>> Handle(GetNativeWordCountQuery request, CancellationToken cancellationToken)
        => Result.Success(await _unitOfWork.NativeWords.GetCountAsync(_currentUser.Id!, cancellationToken));
}
