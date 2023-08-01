using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;

namespace VocaBuddy.UI.ApiHelper;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityApiConfiguration _identityOptions;
    private readonly JsonSerializerOptions _jsonOptions;

    public IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _identityOptions = identityOptions.Value;
        _jsonOptions = new() { PropertyNameCaseInsensitive = true };
    }

    public async Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.LoginEndpoint, loginRequest);

        return await DeserializeResponseAsync<IdentityResult>(response);
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RegisterEndpoint, registrationRequest);

        return await DeserializeResponseAsync<IdentityResult>(response);
    }

    public async Task<IdentityResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RefreshEndpoint, refreshTokenRequest);

        return await DeserializeResponseAsync<IdentityResult>(response);
    }

    private async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        return JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
}
