namespace VocaBuddy.Application.Commands;

public record RecordMistakesCommand(IEnumerable<int> MistakenWordIds) : IRequest<Result>;