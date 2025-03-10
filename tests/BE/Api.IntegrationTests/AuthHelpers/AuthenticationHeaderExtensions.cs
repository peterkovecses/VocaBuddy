using System.Net.Http.Headers;

namespace Api.IntegrationTests.AuthHelpers;

public static class AuthenticationHeaderExtensions
{
    public static HttpRequestHeaders SetAuthorizationHeader(this HttpRequestHeaders requestHeaders, string authToken)
    {
        requestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
        
        return requestHeaders;
    }
}