namespace VocaBuddy.UI.ApiHelpers;

public class IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions) : IIdentityApiClient
{
    private readonly HttpClient _client = client;
    private readonly IdentityApiConfiguration _identityConfig = identityOptions.Value;

    public async Task<Result<TokenHolder>> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.LoginEndpoint, loginRequest);

        return await response.ReadAsAsync<Result<TokenHolder>>();
    }

    public async Task<Result> RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RegisterEndpoint, registrationRequest);
        
        return await response.ReadAsAsync<Result>();
    }

    public async Task<Result<TokenHolder>> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityConfig.RefreshEndpoint, refreshTokenRequest);

        return await response.ReadAsAsync<Result<TokenHolder>>();
    }
}
