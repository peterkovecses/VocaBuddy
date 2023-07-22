namespace VocaBuddy.UI.ApiHelper.IdentityApi;

public class IdentityApiOptions
{
    public string BaseUrl { get; set; }
    public string LoginEndpoint { get; set; }
    public string RegisterEndpoint { get; set; }
    public string RefreshEndpoint { get; set; }
    public string AuthTokenStorageKey { get; set; }
    public string RefreshTokenStorageKey { get; set; }
}
