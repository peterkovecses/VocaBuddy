using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.UI.Extensions;

namespace VocaBuddy.UI.ApiHelpers;

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

    public Task<Result<List<NativeWordDto>>> GetNativeWordsAsync(int? wordCount = default)
    {
        string endpoint;
        if (wordCount is not null)
        {
            endpoint = $"{_vocaBuddyApiConfig.NativeWordsEndpoints}?randomItemCount={wordCount}";
        }
        else
        {
            endpoint = _vocaBuddyApiConfig.NativeWordsEndpoints;
        }

        return GetAsync<Result<List<NativeWordDto>>>(endpoint);
    }

    public Task<Result<NativeWordDto>> GetNativeWordAsync(int id)
        => GetAsync<Result<NativeWordDto>>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

    public Task<Result<int>> GetNativeWordCountAsync()
        => GetAsync<Result<int>>(_vocaBuddyApiConfig.NativeWordsCountEndpoint);

    public Task<Result> CreateNativeWordAsync(NativeWordDto word)
        => PostAsync<Result>(_vocaBuddyApiConfig.NativeWordsEndpoints, word);

    public Task<Result> UpdateNativeWordAsync(NativeWordDto word)
        => PutAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word);

    public Task<Result> DeleteNativeWordAsync(int id)
        => DeleteAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

    private Task<TResult> GetAsync<TResult>(string endpoint)
        => SendRequestAsync<TResult>(HttpMethod.Get, endpoint);

    private Task<TResult> PostAsync<TResult>(string endpoint, object? data = default)
        => SendRequestAsync<TResult>(HttpMethod.Post, endpoint, data);

    private Task<TResult> PutAsync<TResult>(string endpoint, object? data = default)
       => SendRequestAsync<TResult>(HttpMethod.Put, endpoint, data);
    
    private Task<TResult> DeleteAsync<TResult>(string endpoint)
      => SendRequestAsync<TResult>(HttpMethod.Delete, endpoint);

    private async Task<TResult> SendRequestAsync<TResult>(HttpMethod method, string endpoint, object? data = default)
    {
        var response = await ExecuteSendingAsync(method, endpoint, data);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _authService.RefreshTokenAsync();
            response = await ExecuteSendingAsync(method, endpoint, data);
        }

        return await response.ReadAsAsync<TResult>();

        async Task<HttpResponseMessage> ExecuteSendingAsync(HttpMethod method, string endpoint, object? data)
        {
            await SetAuthorizationHeader();
            var request = new HttpRequestMessage(method, endpoint);

            if (data != null)
            {
                request.Content = JsonContent.Create(data);
            }

            return await _client.SendAsync(request);
        }
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        _client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token.TrimQuotationMarks());
    }
}