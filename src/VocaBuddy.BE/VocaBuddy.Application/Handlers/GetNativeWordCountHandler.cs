namespace VocaBuddy.Application.Handlers;

public class GetNativeWordCountHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetNativeWordCountQuery, Result<int>>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly string _currentUserId = currentUser.Id!;

    public async Task<Result<int>> Handle(GetNativeWordCountQuery request, CancellationToken cancellationToken)
        => Result.Success(await _unitOfWork.NativeWords.GetCountAsync(_currentUserId, cancellationToken));
}
