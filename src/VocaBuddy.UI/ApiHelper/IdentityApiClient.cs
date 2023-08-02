using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using System.Text.Json;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelper;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityApiConfiguration _identityConfig;

    public IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _identityConfig = identityOptions.Value;
    }

    public async Task<Result<TokenHolder, IdentityError>> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.LoginEndpoint, loginRequest);

        return await response.DeserializeResponseAsync<Result<TokenHolder, IdentityError>>();
    }

    public async Task<Result<IdentityError>> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RegisterEndpoint, registrationRequest);

        return await response.DeserializeResponseAsync<Result<IdentityError>>();
    }

    public async Task<Result<TokenHolder, IdentityError>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RefreshEndpoint, refreshTokenRequest);

        return await response.DeserializeResponseAsync<Result<TokenHolder, IdentityError>>();
    }
}
