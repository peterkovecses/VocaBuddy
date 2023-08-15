using System.Text.Json;

namespace VocaBuddy.UI.Extensions;

public static class HttpResponseExtensions
{
    private static readonly JsonSerializerOptions _options = new() { PropertyNameCaseInsensitive = true };

    public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response)
    {
        using var responseStream = await response.Content.ReadAsStreamAsync();

        return await JsonSerializer.DeserializeAsync<T>(responseStream, _options);
    }
}
