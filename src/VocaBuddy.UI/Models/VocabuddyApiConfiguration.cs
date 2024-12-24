namespace VocaBuddy.UI.Models;

public class VocaBuddyApiConfiguration
{
    public string BaseUrl { get; set; } = default!;
    public string NativeWordsEndpoints { get; set; } = default!;
    public string RandomNativeWordsEndpoints { get; set; } = default!;
    public string LatestNativeWordsEndpoints { get; set; } = default!;
    public string MistakenNativeWordsEndpoints { get; set; } = default!;
    public string NativeWordsCountEndpoint { get; set; } = default!;
}
