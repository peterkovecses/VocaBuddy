namespace VocaBuddy.Application.Handlers;

public class RecordMistakesHandler(IUnitOfWork unitOfWork, ICurrentUser user) : IRequestHandler<RecordMistakesCommand, Result>
{
    private readonly string _currentUserId = user.Id!;
    
    public async Task<Result> Handle(RecordMistakesCommand request, CancellationToken cancellationToken)
    {
        var mistakes = request.Mistakes.ToDictionary(mistake => mistake.NativeWordId, mistake => mistake.MistakeCount);
        Expression<Func<NativeWord, bool>> predicate = word => word.UserId == _currentUserId && mistakes.ContainsKey(word.Id);
        var words = await unitOfWork.NativeWords.GetAsync(predicate, cancellationToken);
        foreach (var word in words)
        {
            word.MistakeCount += mistakes[word.Id];
        }
        await unitOfWork.CompleteAsync(cancellationToken);
        
        return Result.Success();
    }
}