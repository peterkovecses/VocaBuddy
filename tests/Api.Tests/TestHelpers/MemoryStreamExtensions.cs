namespace Api.Tests.TestHelpers;

public static class MemoryStreamExtensions
{
    public static string Content(this MemoryStream stream)
    {
        stream.Seek(0, SeekOrigin.Begin);
        using var reader = new StreamReader(stream);

        return reader.ReadToEnd();
    }
}
