namespace VocaBuddy.UI.Models;

public class IdentityOptions
{
    public const string Identity = "Apis:Identity";

    public string BaseUrl { get; set; }
    public string LoginEndpoint { get; set; }
    public string RegisterEndpoint { get; set; }
    public string RefreshEndpoint { get; set; }
}
