using System.Text.Json;

namespace VocaBuddy.UI.Extensions;

public static class HttpResponseExtensions
{
    private static readonly JsonSerializerOptions Options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
    {
        await using var responseStream = await response.Content.ReadAsStreamAsync();
        var result = await JsonSerializer.DeserializeAsync<T>(responseStream, Options);

        return result is not null ? result : throw new JsonException("Deserialization resulted in null.");
    }
}
