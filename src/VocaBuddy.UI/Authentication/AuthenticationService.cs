using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using VocaBuddy.Shared.Models;
using VocaBuddy.UI.Interfaces;

namespace VocaBuddy.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly AuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(IIdentityApiClient client,
                              AuthenticationStateProvider authStateProvider,
                              ILocalStorageService localStorage)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);        
        await _localStorage.SetItemAsync("authToken", result!.Token);
        await _localStorage.SetItemAsync("refreshToken", result!.RefreshToken);
        ((CustomAuthenticationStateProvider)_authStateProvider).SignInUser(result.Token);

        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync("authToken");
        ((CustomAuthenticationStateProvider)_authStateProvider).SignOutUser();
    }
}
