namespace VocaBuddy.UI.Services.Authentication;

public class AuthenticationService(
    IIdentityApiClient client,
    CustomAuthenticationStateProvider authStateProvider,
    ILocalStorageService localStorage) : IAuthenticationService
{
    public async Task<Result> LoginAsync(UserLoginRequest loginRequest)
    {
        var result = await client.LoginAsync(loginRequest);

        if (!result.IsSuccess) return result;
        SignInUser(result.Data!);
        await StoreTokensAsync(result.Data!);

        return result;

        void SignInUser(TokenHolder tokens)
            => authStateProvider.SignInUser(tokens.AuthToken);
    }

    public async Task LogoutAsync()
    {
        await localStorage.RemoveItemAsync(ConfigKeys.AuthTokenStorageKey);
        authStateProvider.SignOutUser();
    }

    public async Task<Result> RegisterAsync(UserRegistrationRequestWithPasswordCheck userRegistrationRequest)
        => await client.RegisterAsync(userRegistrationRequest.ConvertToIdentityModel());

    public async Task RefreshTokenAsync()
    {
        var (authToken, refreshToken) = await RetrieveCurrentTokensAsync();
        var result = await client.RefreshTokenAsync(
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
            var authTokenFromStorage = await localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
            var refreshTokenFromStorage = await localStorage.GetItemAsStringAsync(ConfigKeys.RefreshTokenStorageKey);

            return (authTokenFromStorage!.TrimQuotationMarks(), refreshTokenFromStorage!.TrimQuotationMarks());
        }
    }

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        var userIdentity = authState.User.Identity;

        return userIdentity is not null && userIdentity.IsAuthenticated;
    }

    private async Task StoreTokensAsync(TokenHolder tokens)
    {
        await localStorage.SetItemAsync(ConfigKeys.AuthTokenStorageKey, tokens.AuthToken);
        await localStorage.SetItemAsync(ConfigKeys.RefreshTokenStorageKey, tokens.RefreshToken);
    }
}
