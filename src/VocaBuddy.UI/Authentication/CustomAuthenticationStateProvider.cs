using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace VocaBuddy.UI.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILocalStorageService _localStorage;
    private readonly IJwtParser _jwtParser;
    private readonly IdentityApiConfiguration _identityOptions;
    private readonly AuthenticationState _anonymous;

    public CustomAuthenticationStateProvider(
        HttpClient httpClient,
        ILocalStorageService localStorage,
        IJwtParser jwtParser,
        IOptions<IdentityApiConfiguration> identityOptions)
    {
        _httpClient = httpClient;
        _localStorage = localStorage;
        _jwtParser = jwtParser;
        _identityOptions = identityOptions.Value;
        _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await _localStorage.GetItemAsync<string>(_identityOptions.AuthTokenStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return _anonymous;
        }

        _httpClient.DefaultRequestHeaders.Authorization = new("bearer", token);

        return new AuthenticationState(CreateAuthenticatedUser(token));
    }

    public void SignInUser(string token)
    {
        var authenticatedUser = CreateAuthenticatedUser(token);
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void SignOutUser()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
    }

    private ClaimsPrincipal CreateAuthenticatedUser(string token)
        => new(new ClaimsIdentity(_jwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
}
