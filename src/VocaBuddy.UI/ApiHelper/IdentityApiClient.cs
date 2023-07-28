using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using VocaBuddy.UI.JsonHelpers;

namespace VocaBuddy.UI.ApiHelper;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityApiConfiguration _identityOptions;

    public IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _identityOptions = identityOptions.Value;
    }

    public async Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.LoginEndpoint, loginRequest);

        return await DeserializeContent(response);
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RegisterEndpoint, registrationRequest);

        return await DeserializeContent(response);
    }

    public async Task<IdentityResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RefreshEndpoint, refreshTokenRequest);

        return await DeserializeContent(response);
    }

    private static async Task<IdentityResult> DeserializeContent(HttpResponseMessage response)
    {
        var jsonSerializerOptions = CreateSerializerOptions();

        return await response.Content.ReadFromJsonAsync<IdentityResult>(jsonSerializerOptions)
               ?? IdentityResult.Error();
    }

    private static JsonSerializerOptions CreateSerializerOptions()
        => new() { Converters = { new IdentityResultJsonConverter() } };
}
