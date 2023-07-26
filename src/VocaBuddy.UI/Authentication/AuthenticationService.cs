using Blazored.LocalStorage;
using Microsoft.Extensions.Options;

namespace VocaBuddy.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IdentityApiConfiguration _identityOptions;

    public AuthenticationService(
        IIdentityApiClient client,
        CustomAuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _identityOptions = identityOptions.Value;
    }

    public async Task<AuthenticationResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);
        _authStateProvider.SignInUser(result.Token);
        await _localStorage.SetItemAsync(_identityOptions.AuthTokenStorageKey, result!.Token);
        await _localStorage.SetItemAsync(_identityOptions.RefreshTokenStorageKey, result!.RefreshToken);

        return result;
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityOptions.AuthTokenStorageKey);
        _authStateProvider.SignOutUser();
    }

    public async Task RegisterAsync(UserRegistrationRequest userRegistrationRequest)
        => await _client.RegisterAsync(userRegistrationRequest);
}
