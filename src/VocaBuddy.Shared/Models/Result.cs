using System.Text.Json.Serialization;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;

namespace VocaBuddy.Shared.Models;

public class Result
{
    public Result(bool success)
    {
        IsSuccess = success;
        Error = success == false ? new() : default;
    }

    [JsonConstructor]
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

    public static Result Failure()
        => new(false);

    public static Result Failure(ApplicationExceptionBase exception)
        => new(new ErrorInfo(exception.ErrorCode, exception.Message));

    public static Result Failure(ErrorInfo info)
        => new(info);
}

public class Result<TValue> : Result
{
    public TValue Data { get; init; }

    public Result(TValue data) : base(true)
    {
        Data = data;
    }
}