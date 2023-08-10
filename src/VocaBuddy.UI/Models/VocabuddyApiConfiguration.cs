namespace VocaBuddy.UI.Models;

public class VocabuddyApiConfiguration
{
    public string BaseUrl { get; set; }
    public string GetNativeWordsEndpoint { get; set; }
    public string GetNativeWordEndpoint { get; set; }
    public string CreateNativeWordEndpoint { get; set; }
    public string UpdateNativeWordEndpoint { get; set; }
}
