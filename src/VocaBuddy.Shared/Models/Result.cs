using VocaBuddy.Shared.Errors;

namespace VocaBuddy.Shared.Models;

public class Result
{
    public Result(bool success)
    {
        IsSuccess = success;
        Error = success == false ? new() : default;
    }

    public Result(ErrorInfo error)
    {
        Error = error;
    }

    public ErrorInfo? Error { get; init; }
    public bool IsSuccess { get; init; }
    public bool IsError => !IsSuccess;

    public static Result Success()
        => new(true);

    public static Result<TValue> Success<TValue>(TValue data)
        => new(data);

    public static Result BaseError()
        => new(false);
}

public class Result<TValue> : Result
{
    public TValue Data { get; init; }

    public Result(TValue data) : base(true)
    {
        Data = data;
    }
}