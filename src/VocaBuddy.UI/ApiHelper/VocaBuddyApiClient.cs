using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelper;

public class VocaBuddyApiClient : IVocaBuddyApiClient
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly VocabuddyApiConfiguration _vocaBuddyApiConfig;

    public VocaBuddyApiClient(
        HttpClient client,
        IOptions<VocabuddyApiConfiguration> vocaBuddyApiOptions,
        ILocalStorageService localStorage)
    {
        _client = client;
        _localStorage = localStorage;
        _vocaBuddyApiConfig = vocaBuddyApiOptions.Value;
    }

    public async Task<Result<List<NativeWordDto>>> GetNativeWordsAsync()
    {
        await SetAuthorizationHeader();
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

    public async Task<Result> DeleteNativeWordAsync(int id)
    {
        var response = await _client.DeleteAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

        return await response.DeserializeAsync<Result>();
    }

    private async Task SetAuthorizationHeader()
    {
        var tokenWithQuotes = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        var token = tokenWithQuotes.Trim('"');
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
