namespace VocaBuddy.Application.Commands;

public record RecordMistakesCommand(IEnumerable<WordMistake> Mistakes) : IRequest<Result>;