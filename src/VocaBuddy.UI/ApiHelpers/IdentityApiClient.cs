using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using VocaBuddy.Shared.Errors;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelpers;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityApiConfiguration _identityConfig;

    public IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _identityConfig = identityOptions.Value;
    }

    public async Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.LoginEndpoint, loginRequest);

        return await response.ReadAsAsync<Result<TokenHolder>>();
    }

    public async Task<Result> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RegisterEndpoint, registrationRequest);

        return await response.ReadAsAsync<Result<ErrorInfo>>();
    }

    public async Task<Result<TokenHolder>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RefreshEndpoint, refreshTokenRequest);

        return await response.ReadAsAsync<Result<TokenHolder>>();
    }
}
