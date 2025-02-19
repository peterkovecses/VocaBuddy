namespace VocaBuddy.UI.ApiHelpers;

public class IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions) : IIdentityApiClient
{
    private readonly IdentityApiConfiguration _identityConfig = identityOptions.Value;

    public async Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(_identityConfig.LoginEndpoint, loginRequest, cancellationToken);

        return await response.ReadAsAsync<Result<TokenHolder>>(cancellationToken);
    }

    public async Task<Result> RegisterAsync(UserRegistrationRequest registrationRequest, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(_identityConfig.RegisterEndpoint, registrationRequest, cancellationToken);
        
        return await response.ReadAsAsync<Result>(cancellationToken);
    }

    public async Task<Result<TokenHolder>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest, CancellationToken cancellationToken)
    {
        var response = await client.PostAsJsonAsync(_identityConfig.RefreshEndpoint, refreshTokenRequest, cancellationToken);

        return await response.ReadAsAsync<Result<TokenHolder>>(cancellationToken);
    }
}
