using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

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

    public async Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);

        if (LoginSucceed(result))
        {
            SignInUser(result.Data!);
            await StoreTokens(result.Data);
        }

        return result;

        static bool LoginSucceed(IdentityResult result)
            => result.Status == IdentityResultStatus.Success;

        void SignInUser(TokenHolder tokens)
            => _authStateProvider.SignInUser(tokens.AuthToken);

        async Task StoreTokens(TokenHolder tokens)
        {
            await _localStorage.SetItemAsync(_identityOptions.AuthTokenStorageKey, tokens.AuthToken);
            await _localStorage.SetItemAsync(_identityOptions.RefreshTokenStorageKey, tokens.RefreshToken);
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityOptions.AuthTokenStorageKey);
        _authStateProvider.SignOutUser();
    }

    public async Task RegisterAsync(UserRegistrationRequest userRegistrationRequest)
        => await _client.RegisterAsync(userRegistrationRequest);
}
