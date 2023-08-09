using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using Shared.Exceptions;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Exceptions;
using VocaBuddy.Shared.Interfaces;
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

    public async Task LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);
        ValidateResult(result);
        SignInUser(result.Data!);
        await StoreTokensAsync(result.Data!);

        void SignInUser(TokenHolder tokens)
            => _authStateProvider.SignInUser(tokens.AuthToken);

        static void ValidateResult(Result<TokenHolder> result)
        {
            if (result.IsError)
            {
                throw result.Error!.Code switch
                {
                    IdentityErrorCode.InvalidCredentials => new InvalidCredentialsException(),
                    _ => new LoginFailedException(result.Error!.Message),
                };
            }
        }
    }

    public async Task LogoutAsync()
    {
        await _localStorage.RemoveItemAsync(_identityConfig.AuthTokenStorageKey);
        _authStateProvider.SignOutUser();
    }

    public async Task RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest)
    {
        var result = await _client.RegisterAsync(userRegistrationRequest.ConvertToIdentityModel());
        ValidateResult(result);

        static void ValidateResult(Result<ErrorInfo> result)
        {
            if (result.IsError)
            {
                throw result.Error!.Code switch
                {
                    IdentityErrorCode.UserExists => new UserExistsException(),
                    IdentityErrorCode.InvalidUserRegistrationInput => new InvalidUserRegistrationInputException(result.Error!.Message),
                    _ => new RegistrationFailedException(result.Error!.Message),
                };
            }
        }
    }

    public async Task RefreshTokenAsync()
    {
        var (authToken, refreshToken) = await RetrieveCurrentTokensAsync();
        var result = await _client.RefreshTokenAsync(new RefreshTokenRequest { AuthToken = authToken, RefreshToken = refreshToken });

        if (result.IsError)
        {
            throw new RefreshTokenException(result.Error!.Message);
        }

        await StoreTokensAsync(result.Data!);

        async Task<(string authToken, string refreshToken)> RetrieveCurrentTokensAsync()
        {
            var authToken = await _localStorage.GetItemAsStringAsync(_identityConfig.AuthTokenStorageKey);
            var refreshToken = await _localStorage.GetItemAsStringAsync(_identityConfig.RefreshTokenStorageKey);

            return (authToken, refreshToken);
        }
    }

    private async Task StoreTokensAsync(TokenHolder tokens)
    {
        await _localStorage.SetItemAsync(_identityConfig.AuthTokenStorageKey, tokens.AuthToken);
        await _localStorage.SetItemAsync(_identityConfig.RefreshTokenStorageKey, tokens.RefreshToken);
    }
}
