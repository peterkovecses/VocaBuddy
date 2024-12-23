﻿namespace VocaBuddy.UI.Services.Authentication;

public class CustomAuthenticationStateProvider(
    HttpClient httpClient,
    ILocalStorageService localStorage,
    IJwtParser jwtParser,
    IOptions<IdentityApiConfiguration> identityOptions) : AuthenticationStateProvider
{
    private readonly IdentityApiConfiguration _identityOptions = identityOptions.Value;
    private readonly AuthenticationState _anonymous = new(new ClaimsPrincipal(new ClaimsIdentity()));

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await localStorage.GetItemAsync<string>(ConfigKeys.AuthTokenStorageKey);

        if (string.IsNullOrWhiteSpace(token))
        {
            return _anonymous;
        }

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);

        return new AuthenticationState(CreateAuthenticatedUser(token));
    }

    public async Task<bool> IsUserAuthenticatedAsync()
    {
        var authState = await GetAuthenticationStateAsync();
        var userIdentity = authState.User.Identity;

        return userIdentity is not null && userIdentity.IsAuthenticated;
    }

    public void SignInUser(string token)
    {
        var authenticatedUser = CreateAuthenticatedUser(token);
        var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
        NotifyAuthenticationStateChanged(authState);
    }

    public void SignOutUser()
    {
        var authState = Task.FromResult(_anonymous);
        NotifyAuthenticationStateChanged(authState);
    }

    private ClaimsPrincipal CreateAuthenticatedUser(string token)
        => new(new ClaimsIdentity(jwtParser.ParseClaimsFromJwt(token), "jwtAuthType"));
}
