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
        var response = await _client.GetAsync(_vocaBuddyApiConfig.NativeWordsEndpoints);

        return await response.DeserializeAsync<Result<List<NativeWordDto>>>();
    }

    public async Task<Result<NativeWordDto>> GetNativeWordAsync(int id)
    {
        var response = await _client.GetAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

        return await response.DeserializeAsync<Result<NativeWordDto>>();
    }

    public async Task<Result> CreateNativeWord(NativeWordDto word)
    {
        var response = await _client.PostAsJsonAsync(_vocaBuddyApiConfig.NativeWordsEndpoints, word);

        return await response.DeserializeAsync<Result>();
    }

    public async Task<Result> UpdateNativeWord(NativeWordDto word)
    {
        var response = await _client.PutAsJsonAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word);

        return await response.DeserializeAsync<Result>();
    }

    public async Task<Result<NativeWordDto>> DeleteNativeWordAsync(int id)
    {
        var response = await _client.DeleteAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

        return await response.DeserializeAsync<Result<NativeWordDto>>();
    }
}
