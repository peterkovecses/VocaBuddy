namespace VocaBuddy.Api.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public const string IdClaimType = "id";

    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(IdClaimType);
}
