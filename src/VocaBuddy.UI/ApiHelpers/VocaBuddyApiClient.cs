namespace VocaBuddy.UI.ApiHelpers;

public class VocaBuddyApiClient(
    HttpClient client,
    IOptions<VocaBuddyApiConfiguration> vocaBuddyApiOptions,
    ILocalStorageService localStorage,
    IAuthenticationService authService) : IVocaBuddyApiClient
{
    private readonly VocaBuddyApiConfiguration _vocaBuddyApiConfig = vocaBuddyApiOptions.Value;

    public Task<Result<List<NativeWordDto>>> GetNativeWordsAsync()
        => GetAsync<Result<List<NativeWordDto>>>(_vocaBuddyApiConfig.NativeWordsEndpoints);

    public Task<Result<List<CompactNativeWordDto>>> GetRandomNativeWordsAsync(int count)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.RandomNativeWordsEndpoints}?count={count}");

    public Task<Result<List<CompactNativeWordDto>>> GetLatestNativeWordsAsync(int count)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.LatestNativeWordsEndpoints}?count={count}");
    
    public Task<Result<List<CompactNativeWordDto>>> GetMistakenNativeWordsAsync(int count)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.MistakenNativeWordsEndpoints}?count={count}");

    public Task<Result<CompactNativeWordDto>> GetNativeWordAsync(int id)
        => GetAsync<Result<CompactNativeWordDto>>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}");

    public Task<Result<int>> GetNativeWordCountAsync()
        => GetAsync<Result<int>>(_vocaBuddyApiConfig.NativeWordsCountEndpoint);

    public Task<Result> CreateNativeWordAsync(CompactNativeWordDto word)
        => PostAsync<Result>(_vocaBuddyApiConfig.NativeWordsEndpoints, word);

    public Task<Result> UpdateNativeWordAsync(CompactNativeWordDto word)
        => PutAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word);

    public Task<Result> RecordMistakesAsync(IEnumerable<int> mistakenWordIds)
        => PutAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}", mistakenWordIds);

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
        var response = await ExecuteSendingAsync();

        if (response.StatusCode != HttpStatusCode.Unauthorized) return await response.ReadAsAsync<TResult>();
        await authService.RefreshTokenAsync();
        response = await ExecuteSendingAsync();

        return await response.ReadAsAsync<TResult>();

        async Task<HttpResponseMessage> ExecuteSendingAsync()
        {
            await SetAuthorizationHeader();
            var request = new HttpRequestMessage(method, endpoint);

            if (data is not null)
            {
                request.Content = JsonContent.Create(data);
            }

            return await client.SendAsync(request);
        }
    }

    private async Task SetAuthorizationHeader()
    {
        var token = await localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token?.TrimQuotationMarks());
    }
}