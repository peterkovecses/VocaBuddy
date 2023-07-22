namespace VocaBuddy.UI.Authentication.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.Json;

    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = GetPayload(jwt);
            var jsonBytes = DecodeBase64UrlToByteArray(payload);
            var claimsDictionary = DeserializeToDictionary(jsonBytes);

            return GetClaims(claimsDictionary);
        }

        private static IEnumerable<Claim> GetClaims(Dictionary<string, object>? claimsDictionary)
        {
            var claims = new List<Claim>();
            // Roles must be handled differently than plain claims
            AddRolesToClaims(claims, claimsDictionary);
            RemoveExtractedRoles(claimsDictionary);
            AddRemainingClaims(claims, claimsDictionary);

            return claims;
        }

        private static void AddRolesToClaims(List<Claim> claims, Dictionary<string, object> claimsDictionary)
        {
            if (claimsDictionary.TryGetValue(ClaimTypes.Role, out object? roles) && roles != null)
            {
                var parsedRoles = ParseRoles(roles.ToString());
                AddParsedRolesToClaims(claims, parsedRoles);
            }
        }

        private static void AddParsedRolesToClaims(List<Claim> claims, IEnumerable<string> parsedRoles)
        {
            claims.AddRange(parsedRoles.Select(parsedRole => new Claim(ClaimTypes.Role, parsedRole)));
        }

        private static IEnumerable<string> ParseRoles(string rolesStr)
        {
            rolesStr = TrimSquareBrackets(rolesStr);
            var parsedRoles = SplitRoleString(rolesStr);

            return TrimQuotesFromRoles(parsedRoles);
        }

        private static string TrimSquareBrackets(string rolesStr)
            => rolesStr.Trim().TrimStart('[').TrimEnd(']');

        private static string[] SplitRoleString(string rolesStr)
            => rolesStr.Split(',', StringSplitOptions.RemoveEmptyEntries);

        private static IEnumerable<string> TrimQuotesFromRoles(string[] parsedRoles)
            => parsedRoles.Select(role => role.Trim('"'));

        private static void RemoveExtractedRoles(Dictionary<string, object> claimsDictionary)
            => claimsDictionary.Remove(ClaimTypes.Role);

        private static void AddRemainingClaims(List<Claim> claims, Dictionary<string, object> claimsDictionary)
            => claims.AddRange(claimsDictionary.Select(keyValuePair => new Claim(keyValuePair.Key, keyValuePair.Value.ToString())));

        private static Dictionary<string, object> DeserializeToDictionary(byte[] jsonBytes)
            => JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

        private static string GetPayload(string jwt)
            => jwt.Split('.')[1];

        private static byte[] DecodeBase64UrlToByteArray(string payload)
        {
            var base64UrlWithPadding = AddPaddingToBase64Url(payload);

            return Convert.FromBase64String(base64UrlWithPadding);
        }

        private static string AddPaddingToBase64Url(string base64Url)
        {
            // Add missing padding characters to the end of the Base64Url encoded string
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
}