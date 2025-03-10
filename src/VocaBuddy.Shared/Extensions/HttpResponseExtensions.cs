using Newtonsoft.Json;

namespace VocaBuddy.Shared.Extensions;

public static class HttpResponseExtensions
{
    private static readonly JsonSerializerSettings Settings = new()
    {
        NullValueHandling = NullValueHandling.Ignore,
        MissingMemberHandling = MissingMemberHandling.Ignore
    };

    public static async Task<T> ReadAsAsync<T>(this HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var responseString = await response.Content.ReadAsStringAsync(cancellationToken);
        var result = JsonConvert.DeserializeObject<T>(responseString, Settings);

        return result is not null 
            ? result 
            : throw new JsonSerializationException("Deserialization resulted in null.");
    }
}
