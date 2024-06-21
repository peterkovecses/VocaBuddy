namespace VocaBuddy.Shared.Constants;

public static class ValidationConstants
{
    public const int MaxWordLength = 30;

    public static readonly string AboveMaxLengthMessage = $"The number of characters in the word must not exceed {ValidationConstants.MaxWordLength}";
}
