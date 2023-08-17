using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelper;

public class VocaBuddyApiClient : IVocaBuddyApiClient
{
    private readonly HttpClient _client;
    private readonly ILocalStorageService _localStorage;
    private readonly IAuthenticationService _authService;
    private readonly VocabuddyApiConfiguration _vocaBuddyApiConfig;

    public VocaBuddyApiClient(
        HttpClient client,
        IOptions<VocabuddyApiConfiguration> vocaBuddyApiOptions,
        ILocalStorageService localStorage,
        IAuthenticationService authService)
    {
        _client = client;
        _localStorage = localStorage;
        _authService = authService;
        _vocaBuddyApiConfig = vocaBuddyApiOptions.Value;
    }

    public async Task<Result<List<NativeWordDto>>> GetNativeWordsAsync()
    {
        var response = await SendRequest();

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await SendRequest();            
        }
       
        return await response.ReadAsAsync<Result<List<NativeWordDto>>>();

        async Task<HttpResponseMessage> SendRequest()
        {
            await SetAuthorizationHeader();
            
            return await _client.GetAsync(_vocaBuddyApiConfig.NativeWordsEndpoints);
        }
    }

    public async Task<Result<NativeWordDto>> GetNativeWordAsync(int id)
    {
        var response = await SendRequest(id);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await SendRequest(id);
        }

        return await response.ReadAsAsync<Result<NativeWordDto>>();

        async Task<HttpResponseMessage> SendRequest(int id)
        {
            await SetAuthorizationHeader();

            return await _client.GetAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");
        }
    }

    public async Task<Result> CreateNativeWord(NativeWordDto word)
    {
        var response = await SendRequest(word);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await SendRequest(word);
        }

        return await response.ReadAsAsync<Result>();

        async Task<HttpResponseMessage> SendRequest(NativeWordDto word)
        {
            await SetAuthorizationHeader();

            return await _client.PostAsJsonAsync(_vocaBuddyApiConfig.NativeWordsEndpoints, word);
        }
    }

    public async Task<Result> UpdateNativeWord(NativeWordDto word)
    {
        var response = await SendRequest(word);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await SendRequest(word);
        }

        return await response.ReadAsAsync<Result>();

        async Task<HttpResponseMessage> SendRequest(NativeWordDto word)
        {
            await SetAuthorizationHeader();

            return await _client.PutAsJsonAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word);
        }
    }

    public async Task<Result> DeleteNativeWordAsync(int id)
    {
        var response = await SendRequest(id);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await SendRequest(id);
        }

        return await response.ReadAsAsync<Result>();

        async Task<HttpResponseMessage> SendRequest(int id)
        {
            await SetAuthorizationHeader();

            return await _client.DeleteAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");
        }
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.TrimQuotationMarks());
    }
}
