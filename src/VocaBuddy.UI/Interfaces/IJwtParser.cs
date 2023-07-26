using System.Security.Claims;

namespace VocaBuddy.UI.Interfaces
{
    public interface IJwtParser
    {
        IEnumerable<Claim> ParseClaimsFromJwt(string jwt);
    }
}