using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Options;
using VocaBuddy.Shared.Models;
using VocaBuddy.UI.Interfaces;
using VocaBuddy.UI.Models;

namespace VocaBuddy.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IdentityOptions _identityOptions;

    public AuthenticationService(IIdentityApiClient client,
                              AuthenticationStateProvider authStateProvider,
                              ILocalStorageService localStorage,
                              IOptions<IdentityOptions> identityOptions)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _identityOptions = identityOptions.Value;
    }

    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);        
        await _localStorage.SetItemAsync(_identityOptions.AuthTokenStorageKey, result!.Token);
        await _localStorage.SetItemAsync(_identityOptions.RefreshTokenStorageKey, result!.RefreshToken);
        ((CustomAuthenticationStateProvider)_authStateProvider).SignInUser(result.Token);

        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityOptions.AuthTokenStorageKey);
        ((CustomAuthenticationStateProvider)_authStateProvider).SignOutUser();
    }
}
