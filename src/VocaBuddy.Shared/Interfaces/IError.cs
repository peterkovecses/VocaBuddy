namespace VocaBuddy.Shared.Interfaces;

public interface IError
{
    string Code { get; init; }
    string Message { get; init; }
}
