namespace Identity.Infrastructure.Persistence;

public class IdentityContext(
    DbContextOptions options,
    IOptions<OperationalStoreOptions> operationalStoreOptions)
    : ApiAuthorizationDbContext<ApplicationUser>(options, operationalStoreOptions), IIdentityContext
{
    public DbSet<CustomRefreshToken> RefreshTokens { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(IdentityContext).Assembly);
    }
}