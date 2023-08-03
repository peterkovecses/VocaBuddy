using Microsoft.Extensions.Options;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Interfaces;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelper;

public class VocaBuddyApiClient : IVocaBuddyApiClient
{
    private readonly HttpClient _client;
    private readonly VocabuddyApiConfiguration _vocaBuddyApiConfig;

    public VocaBuddyApiClient(HttpClient client, IOptions<VocabuddyApiConfiguration> vocaBuddyApiOptions)
    {
        _client = client;
        _vocaBuddyApiConfig = vocaBuddyApiOptions.Value;
    }

    public async Task<Result<List<NativeWordDto>, IError>> GetNativeWordsAsync()
    {
        var response = await _client.GetAsync(_vocaBuddyApiConfig.GetNativeWordsEndpoint);

        return await response.DeserializeResponseAsync<Result<List<NativeWordDto>, IError>>();
    }
}
