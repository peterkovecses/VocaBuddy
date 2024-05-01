namespace VocaBuddy.Shared.Models;

public class Result
{
    public ErrorInfo? ErrorInfo { get; init; }
    public bool IsSuccess => ErrorInfo is null;
    public bool IsFailure => !IsSuccess;

    public static Result Success()
        => new();

    public static Result<TData> Success<TData>(TData data)
        => new(data);

    public static Result Failure(ErrorInfo error)
        => new() { ErrorInfo = error };

    public static Result<TData> Failure<TData>(ErrorInfo error)
        => new(error);

    public static Result ServerError()
        => Failure(ErrorInfo.ServerError());
}

public class Result<TData> : Result
{
    public TData? Data { get; init; }

    public Result() { }

    public Result(TData data)
    {
        Data = data;
    }

    public Result(ErrorInfo error)
    {
        ErrorInfo = error;
    }
}