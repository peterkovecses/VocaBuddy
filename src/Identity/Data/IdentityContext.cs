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
