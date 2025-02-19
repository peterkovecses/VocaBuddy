namespace VocaBuddy.UI.ApiHelpers;

public class VocaBuddyApiClient(
    HttpClient client,
    IOptions<VocaBuddyApiConfiguration> vocaBuddyApiOptions,
    ILocalStorageService localStorage,
    IAuthenticationService authService) : IVocaBuddyApiClient
{
    private readonly VocaBuddyApiConfiguration _vocaBuddyApiConfig = vocaBuddyApiOptions.Value;

    public Task<Result<List<NativeWordDto>>> GetNativeWordsAsync(CancellationToken cancellationToken = default)
        => GetAsync<Result<List<NativeWordDto>>>(_vocaBuddyApiConfig.NativeWordsEndpoints, cancellationToken);

    public Task<Result<List<CompactNativeWordDto>>> GetRandomNativeWordsAsync(int count, CancellationToken cancellationToken = default)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.RandomNativeWordsEndpoints}?count={count}", cancellationToken);

    public Task<Result<List<CompactNativeWordDto>>> GetLatestNativeWordsAsync(int count, CancellationToken cancellationToken = default)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.LatestNativeWordsEndpoints}?count={count}", cancellationToken);
    
    public Task<Result<List<CompactNativeWordDto>>> GetMistakenNativeWordsAsync(int count, CancellationToken cancellationToken = default)
        => GetAsync<Result<List<CompactNativeWordDto>>>($"{_vocaBuddyApiConfig.MistakenNativeWordsEndpoints}?count={count}", cancellationToken);

    public Task<Result<CompactNativeWordDto>> GetNativeWordAsync(int id, CancellationToken cancellationToken = default)
        => GetAsync<Result<CompactNativeWordDto>>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}", cancellationToken);

    public Task<Result<int>> GetNativeWordCountAsync(CancellationToken cancellationToken = default)
        => GetAsync<Result<int>>(_vocaBuddyApiConfig.NativeWordsCountEndpoint, cancellationToken);

    public Task<Result> CreateNativeWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken = default)
        => PostAsync<Result>(_vocaBuddyApiConfig.NativeWordsEndpoints, word, cancellationToken);

    public Task<Result> UpdateNativeWordAsync(CompactNativeWordDto word, CancellationToken cancellationToken = default)
        => PutAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{word.Id}", word, cancellationToken);

    public Task<Result> RecordMistakesAsync(IEnumerable<int> mistakenWordIds, CancellationToken cancellationToken = default)
        => PutAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}", mistakenWordIds, cancellationToken);

    public Task<Result> DeleteNativeWordAsync(int id, CancellationToken cancellationToken = default)
        => DeleteAsync<Result>($"{_vocaBuddyApiConfig.NativeWordsEndpoints}/{id}", cancellationToken);

    private Task<TResult> GetAsync<TResult>(string endpoint, CancellationToken cancellationToken = default)
        => SendRequestAsync<TResult>(HttpMethod.Get, endpoint, cancellationToken);

    private Task<TResult> PostAsync<TResult>(string endpoint, object? data = default, CancellationToken cancellationToken = default)
        => SendRequestAsync<TResult>(HttpMethod.Post, endpoint, cancellationToken, data);

    private Task<TResult> PutAsync<TResult>(string endpoint, object? data = default, CancellationToken cancellationToken = default)
       => SendRequestAsync<TResult>(HttpMethod.Put, endpoint, cancellationToken, data);
    
    private Task<TResult> DeleteAsync<TResult>(string endpoint, CancellationToken cancellationToken = default)
      => SendRequestAsync<TResult>(HttpMethod.Delete, endpoint, cancellationToken);

    private async Task<TResult> SendRequestAsync<TResult>(HttpMethod method, string endpoint, CancellationToken cancellationToken, object? data = default)
    {
        var response = await ExecuteSendingAsync(cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized) return await response.ReadAsAsync<TResult>(cancellationToken);
        await authService.RefreshTokenAsync(cancellationToken);
        response = await ExecuteSendingAsync(cancellationToken);

        return await response.ReadAsAsync<TResult>(cancellationToken);

        async Task<HttpResponseMessage> ExecuteSendingAsync(CancellationToken ct)
        {
            await SetAuthorizationHeaderAsync();
            var request = new HttpRequestMessage(method, endpoint);

            if (data is not null)
            {
                request.Content = JsonContent.Create(data);
            }

            return await client.SendAsync(request, ct);
        }
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await localStorage.GetItemAsStringAsync(ConfigKeys.AuthTokenStorageKey);
        client.DefaultRequestHeaders.Authorization
            = new AuthenticationHeaderValue("Bearer", token?.TrimQuotationMarks());
    }
}