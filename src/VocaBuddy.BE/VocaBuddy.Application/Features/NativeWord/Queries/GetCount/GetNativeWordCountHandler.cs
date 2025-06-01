namespace VocaBuddy.Application.Features.NativeWord.Queries.GetCount;

public class GetNativeWordCountHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetNativeWordCountQuery, Result<int>>
{
    private readonly string _currentUserId = currentUser.Id!;

    public async Task<Result<int>> Handle(GetNativeWordCountQuery request, CancellationToken cancellationToken)
        => Result.Success(await unitOfWork.NativeWords.GetCountAsync(_currentUserId, cancellationToken));
}
