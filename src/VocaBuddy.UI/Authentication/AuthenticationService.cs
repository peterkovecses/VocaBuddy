using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using VocaBuddy.UI.Exceptions;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;

    public AuthenticationService(
        IIdentityApiClient client,
        CustomAuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
    }

    public async Task<Result> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);
        
        if (result.IsSuccess)
        {
            SignInUser(result.Data!);
            await StoreTokensAsync(result.Data!);
        }

        return result;

        void SignInUser(TokenHolder tokens)
            => _authStateProvider.SignInUser(tokens.AuthToken);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(ConfigKeys.AuthTokenStorageKey);
        _authStateProvider.SignOutUser();
    }

    public async Task<Result> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest)
        => await _client.RegisterAsync(userRegistrationRequest.ConvertToIdentityModel());

    public async Task RefreshTokenAsync()
    {
        var (authToken, refreshToken) = await RetrieveCurrentTokensAsync();
        var result = await _client.RefreshTokenAsync(new RefreshTokenRequest { AuthToken = authToken, RefreshToken = refreshToken });

        if (result.IsError)
        {
            throw new RefreshTokenException(result.Error!);
        }

        await StoreTokensAsync(result.Data!);

        async Task<(string authToken, string refreshToken)> RetrieveCurrentTokensAsync()
        {
            var authToken = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
            var refreshToken = await _localStorage.GetItemAsStringAsync(ConfigKeys.RefreshTokenStorageKey);

            return (authToken.TrimQuotationMarks(), refreshToken.TrimQuotationMarks());
        }
    }

    private async Task StoreTokensAsync(TokenHolder tokens)
    {
        await _localStorage.SetItemAsync(ConfigKeys.AuthTokenStorageKey, tokens.AuthToken);
        await _localStorage.SetItemAsync(ConfigKeys.RefreshTokenStorageKey, tokens.RefreshToken);
    }
}
