namespace VocaBuddy.Application.Features.NativeWord.Commands.RecordMistakes;

public record RecordMistakesCommand(IEnumerable<int> MistakenWordIds) : IRequest<Result>;