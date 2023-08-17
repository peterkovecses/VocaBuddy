﻿using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Errors;
using VocaBuddy.Shared.Models;
using VocaBuddy.UI.Exceptions;
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
            throw new RefreshTokenException(new ErrorInfo("test code", "test message"));

            await _authService.RefreshTokenAsync();
            response = await SendRequest();            
        }
       
        return await response.ReadAsAsync<Result<List<NativeWordDto>>>();

        async Task<HttpResponseMessage> SendRequest()
        {
            await SetAuthorizationHeader();
            var response = await _client.GetAsync(_vocaBuddyApiConfig.NativeWordsEndpoints);
            return response;
        }
    }

    public async Task<Result<NativeWordDto>> GetNativeWordAsync(int id)
    {
        var response = await _client.GetAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

        return await response.ReadAsAsync<Result<NativeWordDto>>();
    }

    public async Task<Result> CreateNativeWord(NativeWordDto word)
    {
        var response = await _client.PostAsJsonAsync(_vocaBuddyApiConfig.NativeWordsEndpoints, word);

        return await response.ReadAsAsync<Result>();
    }

    public async Task<Result> UpdateNativeWord(NativeWordDto word)
    {
        var response = await _client.PutAsJsonAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word);

        return await response.ReadAsAsync<Result>();
    }

    public async Task<Result> DeleteNativeWordAsync(int id)
    {
        var response = await _client.DeleteAsync($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

        return await response.ReadAsAsync<Result>();
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await _localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token.TrimQuotationMarks());
    }
}
