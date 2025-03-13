namespace Api.IntegrationTests;

public abstract class IntegrationTestBase(VocaBuddyApiFactory apiFactory)
{
    private static readonly Random Random = new();
    protected readonly HttpClient Client = apiFactory.CreateClient();

    protected void SetAuthHeader(int? userId = default)
    {
        userId ??= Random.Next(1, 1000);
        var authToken = JwtHelpers.GenerateToken(userId);
        Client.DefaultRequestHeaders.SetAuthorizationHeader(authToken);
    }
}