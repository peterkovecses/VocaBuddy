using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using VocaBuddy.UI.Exceptions;
using VocaBuddy.UI.Interfaces;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Api.IdentityApi;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityOptions _identityOptions;

    public IdentityApiClient(HttpClient client, IOptions<IdentityOptions> identityOptions)
    {
        _client = client;
        _identityOptions = identityOptions.Value;
    }

    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.LoginEndpoint, loginRequest);
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<AuthenticationResult>();
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            throw new InvalidCredentialsException();
        }

        throw new HttpRequestException();
    }

    public async Task RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RegisterEndpoint, registrationRequest);
        response.EnsureSuccessStatusCode();
    }

    public async Task<AuthenticationResult> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RefreshEndpoint, refreshTokenRequest);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<AuthenticationResult>();
    }
}
