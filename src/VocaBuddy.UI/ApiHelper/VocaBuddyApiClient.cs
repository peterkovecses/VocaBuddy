using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using VocaBuddy.Shared.Dtos;
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

    public async Task<Result<List<NativeWordDto>>> GetNativeWordsAsync()
    {
        var response = await _client.GetAsync(_vocaBuddyApiConfig.GetNativeWordsEndpoint);

        return await response.DeserializeAsync<Result<List<NativeWordDto>>>();
    }

    public async Task<Result> CreateNativeWord(NativeWordDto word)
    {
        var response = await _client.PostAsJsonAsync(_vocaBuddyApiConfig.CreateNativeWordEndpoint, word);

        return await response.DeserializeAsync<Result>();
    }
}
