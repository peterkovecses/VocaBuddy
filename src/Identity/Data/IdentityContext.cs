using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Identity.Data;

public class IdentityContext : ApiAuthorizationDbContext<IdentityUser>
{
    public IdentityContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
    }

    public DbSet<CustomRefreshToken> RefreshTokens { get; set; } = default!;
}
