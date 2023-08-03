using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Interfaces;
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

    public async Task<Result<TokenHolder, BaseError>> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.LoginEndpoint, loginRequest);

        return await response.DeserializeResponseAsync<Result<TokenHolder, BaseError>>();
    }

    public async Task<Result<BaseError>> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RegisterEndpoint, registrationRequest);

        var result = await response.DeserializeResponseAsync<Result<BaseError>>();

        return result;
    }

    public async Task<Result<TokenHolder, BaseError>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RefreshEndpoint, refreshTokenRequest);

        return await response.DeserializeResponseAsync<Result<TokenHolder, BaseError>>();
    }
}
