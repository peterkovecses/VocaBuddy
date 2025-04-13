namespace VocaBuddy.Application.Features.NativeWords.Commands.RecordMistakes;

public class RecordMistakesHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<RecordMistakesCommand, Result>
{
    private readonly string _currentUserId = user.Id!;
    
    public async Task<Result> Handle(RecordMistakesCommand request, CancellationToken cancellationToken)
    {
        await unitOfWork.NativeWords.RecordMistakesAsync(_currentUserId, request.MistakenWordIds, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        
        return Result.Success();
    }
}