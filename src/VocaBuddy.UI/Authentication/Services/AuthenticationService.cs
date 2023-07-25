using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using VocaBuddy.UI.ApiHelper.IdentityApi;
using VocaBuddy.UI.Authentication.Models;

namespace VocaBuddy.UI.Authentication.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IdentityApiOptions _identityOptions;

    public AuthenticationService(
        IIdentityApiClient client,
        AuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IOptions<IdentityApiOptions> identityOptions)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _identityOptions = identityOptions.Value;
    }

    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);
        ((CustomAuthenticationStateProvider)_authStateProvider).SignInUser(result.Token);
        await _localStorage.SetItemAsync(_identityOptions.AuthTokenStorageKey, result!.Token);
        await _localStorage.SetItemAsync(_identityOptions.RefreshTokenStorageKey, result!.RefreshToken);

        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityOptions.AuthTokenStorageKey);
        ((CustomAuthenticationStateProvider)_authStateProvider).SignOutUser();
    }

    public async Task RegisterAsync(UserRegistrationRequest userRegistrationRequest)
    {
        await _client.RegisterAsync(userRegistrationRequest);
    }
}
