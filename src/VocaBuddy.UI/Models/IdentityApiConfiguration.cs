namespace VocaBuddy.UI.Models;

public class IdentityApiConfiguration
{
    public string BaseUrl { get; set; }
    public string LoginEndpoint { get; set; }
    public string RegisterEndpoint { get; set; }
    public string RefreshEndpoint { get; set; }
    public string RoleKey { get; set; }
}
