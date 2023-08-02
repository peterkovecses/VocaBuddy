using System.Text.Json;

namespace VocaBuddy.UI.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<T> DeserializeResponseAsync<T>(this HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
