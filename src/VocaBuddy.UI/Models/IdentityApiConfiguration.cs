namespace VocaBuddy.UI.Models;

public class IdentityApiConfiguration
{
    public string BaseUrl { get; set; } = default!;
    public string LoginEndpoint { get; set; } = default!;
    public string RegisterEndpoint { get; set; } = default!;
    public string RefreshEndpoint { get; set; } = default!;
    public string RoleKey { get; set; } = default!;
}
