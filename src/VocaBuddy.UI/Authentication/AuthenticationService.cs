﻿using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using VocaBuddy.UI.Exceptions;

namespace VocaBuddy.UI.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IIdentityApiClient _client;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IdentityApiConfiguration _identityConfig;

    public AuthenticationService(
        IIdentityApiClient client,
        CustomAuthenticationStateProvider authStateProvider,
        ILocalStorageService localStorage,
        IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _authStateProvider = authStateProvider;
        _localStorage = localStorage;
        _identityConfig = identityOptions.Value;
    }

    public async Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);

        if (SuccessfulResult(result))
        {
            SignInUser(result.Tokens!);
            await StoreTokensAsync(result.Tokens!);
        }

        return result;

        void SignInUser(TokenHolder tokens)
            => _authStateProvider.SignInUser(tokens.AuthToken);
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityConfig.AuthTokenStorageKey);
        _authStateProvider.SignOutUser();
    }

    public async Task<IdentityResult> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest)
        => await _client.RegisterAsync(userRegistrationRequest.ConvertToIdentityModel());

    public async Task RefreshTokenAsync()
    {
        var (authToken, refreshToken) = await RetrieveCurrentTokensAsync();
        var result = await _client.RefreshTokenAsync(new RefreshTokenRequest { AuthToken = authToken, RefreshToken = refreshToken });

        if (SuccessfulResult(result))
        {
            throw new RefreshTokenException(result.ErrorMessage!);
        }

        await StoreTokensAsync(result.Tokens!);

        async Task<(string authToken, string refreshToken)> RetrieveCurrentTokensAsync()
        {
            var authToken = await _localStorage.GetItemAsStringAsync(_identityConfig.AuthTokenStorageKey);
            var refreshToken = await _localStorage.GetItemAsStringAsync(_identityConfig.RefreshTokenStorageKey);

            return (authToken, refreshToken);
        }
    }

    private static bool SuccessfulResult(IdentityResult result)
    => result.Status == IdentityResultStatus.Success;

    private async Task StoreTokensAsync(TokenHolder tokens)
    {
        await _localStorage.SetItemAsync(_identityConfig.AuthTokenStorageKey, tokens.AuthToken);
        await _localStorage.SetItemAsync(_identityConfig.RefreshTokenStorageKey, tokens.RefreshToken);
    }
}
