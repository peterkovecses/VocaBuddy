using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Json;

namespace VocaBuddy.UI.Services.Authentication;

public class JwtParser : IJwtParser
{
    private readonly IdentityApiConfiguration _identityConfiguration;

    public JwtParser(IOptions<IdentityApiConfiguration> identityOptions)
    {
        _identityConfiguration = identityOptions?.Value ?? throw new ArgumentNullException(nameof(identityOptions));
    }

    public IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        if (string.IsNullOrEmpty(jwt))
        {
            throw new ArgumentException("JWT token must not be null or empty.", nameof(jwt));
        }

        var payload = GetPayload(jwt);
        var jsonBytes = DecodeBase64UrlToByteArray(payload);
        var claimsDictionary = DeserializeToDictionary(jsonBytes);

        return GetClaims(claimsDictionary);
    }

    private IEnumerable<Claim> GetClaims(Dictionary<string, object> claimsDictionary)
    {
        var claims = new List<Claim>();
        AddRolesToClaims(claims, claimsDictionary);
        RemoveExtractedRoles(claimsDictionary);
        AddRemainingClaims(claims, claimsDictionary);

        return claims;
    }

    private void AddRolesToClaims(List<Claim> claims, IReadOnlyDictionary<string, object> claimsDictionary)
    {
        if (claimsDictionary.TryGetValue(_identityConfiguration.RoleKey, out var roles))
        {
            var parsedRoles = ParseRoles(roles.ToString()!);
            AddParsedRolesToClaims(claims, parsedRoles);
        }
    }

    private static IEnumerable<string> ParseRoles(string rolesStr)
        => IsArray(rolesStr)
            ? JsonSerializer.Deserialize<IEnumerable<string>>(rolesStr)!
            : new List<string> { rolesStr };

    private static bool IsArray(string rolesStr)
        => rolesStr.StartsWith("[") && rolesStr.EndsWith("]");

    private static void AddParsedRolesToClaims(List<Claim> claims, IEnumerable<string> parsedRoles)
        => claims.AddRange(parsedRoles.Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));

    private void RemoveExtractedRoles(IDictionary<string, object> claimsDictionary)
        => claimsDictionary.Remove(_identityConfiguration.RoleKey);

    private static void AddRemainingClaims(List<Claim> claims, Dictionary<string, object> claimsDictionary)
        => claims.AddRange(claimsDictionary.Select(keyValuePair => new Claim(keyValuePair.Key, keyValuePair.Value.ToString()!)));

    private static Dictionary<string, object> DeserializeToDictionary(byte[] jsonBytes)
        => JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes)!;

    private static string GetPayload(string jwt)
    {
        var jwtParts = jwt.Split('.');
        if (jwtParts.Length != 3)
        {
            throw new ArgumentException("Invalid JWT token format.", nameof(jwt));
        }

        return jwtParts[1];
    }

    private static byte[] DecodeBase64UrlToByteArray(string payload)
    {
        var base64UrlWithPadding = AddPaddingToBase64Url(payload);

        return Convert.FromBase64String(base64UrlWithPadding);
    }

    private static string AddPaddingToBase64Url(string base64Url)
    {
        base64Url += (base64Url.Length % 4) switch
        {
            0 => "",
            2 => "==",
            3 => "=",
            _ => throw new FormatException("Invalid Base64Url encoding."),
        };

        return base64Url;
    }
}