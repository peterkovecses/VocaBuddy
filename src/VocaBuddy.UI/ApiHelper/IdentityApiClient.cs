using Microsoft.Extensions.Options;
using Shared.Exceptions;
using System.Net.Http.Json;
using System.Text.Json;
using VocaBuddy.UI.JsonHelpers;

namespace VocaBuddy.UI.ApiHelper;

public class IdentityApiClient : IIdentityApiClient
{
    private readonly HttpClient _client;
    private readonly IdentityApiConfiguration _identityOptions;

    public IdentityApiClient(HttpClient client, IOptions<IdentityApiConfiguration> identityOptions)
    {
        _client = client;
        _identityOptions = identityOptions.Value;
    }

    public async Task<IdentityResult> LoginAsync(UserLoginRequest loginRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.LoginEndpoint, loginRequest);

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            Converters =
            {
                new IdentityResultJsonConverter(),
                new TokenHolderJsonConverter()
            }
        };

        return await response.Content.ReadFromJsonAsync<IdentityResult>(jsonSerializerOptions)
               ?? IdentityResult.Error();
    }

    public async Task RegisterAsync(UserRegistrationRequest registrationRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RegisterEndpoint, registrationRequest);

        if (response.IsSuccessStatusCode)
        {
            return;
        }

        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        {
            var errorMessage = await GetErrorMessage(response.Content);

            throw new UserCreationException(errorMessage);
        }

        throw new HttpRequestException();

        static async Task<string> GetErrorMessage(HttpContent content)
        {
            var stringContent = await content.ReadAsStringAsync();
            var responseObject = JsonDocument.Parse(stringContent).RootElement;
            var errorMessage = responseObject.GetProperty("error").GetString();

            return errorMessage;
        }
    }

    public async Task<TokenHolder> RefreshTokenAsync(RefreshTokenRequest refreshTokenRequest)
    {
        var response = await _client.PostAsJsonAsync(_identityOptions.RefreshEndpoint, refreshTokenRequest);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<TokenHolder>();
    }
}
