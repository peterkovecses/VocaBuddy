namespace VocaBuddy.Shared.Models;

public abstract class Result
{
    public const string ServerErrorMessage = "An error occurred while processing the request.";

    public string? ErrorMessage { get; init; }
}

public class Result<TError> : Result where TError : struct
{
    public TError? Error { get; init; }
    public bool IsSuccess => Error is null;
    public bool IsError => !IsSuccess;

    public static Result<TError> Success()
        => new();

    public static Result<TError> FromError(TError error, string errorMessage)
        => new()
        {
            Error = error,
            ErrorMessage = errorMessage
        };

    public static Result<TError> ServerError(TError error)
        => new()
        {
            Error = error,
            ErrorMessage = ServerErrorMessage
        };
}

public class Result<TValue, TError> : Result<TError> where TError : struct
{
    public TValue? Data { get; init; }

    public static new Result<TValue, TError> Success()
        => new();

    public static Result<TValue, TError> Success(TValue data)
        => new()
        {
            Data = data
        };

    public static new Result<TValue, TError> FromError(TError error, string errorMessage)
        => new()
        {
            Error = error,
            ErrorMessage = errorMessage
        };

    public static new Result<TValue, TError> ServerError(TError error)
        => new()
        {
            Error = error,
            ErrorMessage = ServerErrorMessage
        };
}
