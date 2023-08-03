using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;

namespace VocaBuddy.Shared.Models;

public class Result
{
    public Result(bool success)
    {
        IsSuccess = success;
    }

    public bool IsSuccess { get; init; }
    public bool IsError => !IsSuccess;

    public static Result Success()
        => new(true);

    public static Result<TValue, BaseError> Success<TValue>(TValue data)
        => new(data);

    public static Result<BaseError> Failure(BaseError error)
        => new(error);
}

public class Result<TError> : Result where TError : IError
{
    public TError? Error { get; init; }

    public Result() : base(true)
    {

    }

    public Result(TError error) : base(false)
    {
        Error = error;
    }
}

public class Result<TValue, TError> : Result<TError> where TError : IError
{
    public TValue Data { get; init; }

    public Result(TValue data) : base()
    {
        Data = data;
    }
}
