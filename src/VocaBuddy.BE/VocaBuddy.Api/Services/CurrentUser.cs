using System.Security.Claims;

namespace VocaBuddy.Api.Services;

public class CurrentUser : ICurrentUser
{
    public const string IdClaimType = "Id";

    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string? Id => _httpContextAccessor.HttpContext?.User.FindFirstValue(IdClaimType);
}
