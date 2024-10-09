namespace VocaBuddy.UI.Services.Authentication;

public class AuthenticationService(
    IIdentityApiClient client,
    CustomAuthenticationStateProvider authStateProvider,
    ILocalStorageService localStorage) : IAuthenticationService
{
    private readonly IIdentityApiClient _client = client;
    private readonly CustomAuthenticationStateProvider _authStateProvider = authStateProvider;
    private readonly ILocalStorageService _localStorage = localStorage;

    public async Task<Result> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await _client.LoginAsync(loginRequest);

        if (!result.IsSuccess) return result;
        SignInUser(result.Data!);
        await StoreTokensAsync(result.Data!);

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
        var result = await _client.RefreshTokenAsync(
            new RefreshTokenRequest { AuthToken = authToken, RefreshToken = refreshToken });

        if (result.IsFailure)
        {
            var errors = string.Join(';', result.ErrorInfo!.Errors.Select(error => error.Message));
            throw new RefreshTokenException(errors);
        }

        await StoreTokensAsync(result.Data!);

        return;

        async Task<(string authToken, string refreshToken)> RetrieveCurrentTokensAsync()
        {
            var authTokenFromStorage = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
            var refreshTokenFromStorage = await _localStorage.GetItemAsStringAsync(ConfigKeys.RefreshTokenStorageKey);

            return (authTokenFromStorage!.TrimQuotationMarks(), refreshTokenFromStorage!.TrimQuotationMarks());
        }
    }

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var authState = await _authStateProvider.GetAuthenticationStateAsync();
        var userIdentity = authState.User.Identity;

        return userIdentity is not null && userIdentity.IsAuthenticated;
    }

    private async Task StoreTokensAsync(TokenHolder tokens)
    {
        await _localStorage.SetItemAsync(ConfigKeys.AuthTokenStorageKey, tokens.AuthToken);
        await _localStorage.SetItemAsync(ConfigKeys.RefreshTokenStorageKey, tokens.RefreshToken);
    }
}
