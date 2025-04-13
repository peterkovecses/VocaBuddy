namespace VocaBuddy.Application.Features.NativeWords.Commands.RecordMistakes;

public record RecordMistakesCommand(IEnumerable<int> MistakenWordIds) : IRequest<Result>;