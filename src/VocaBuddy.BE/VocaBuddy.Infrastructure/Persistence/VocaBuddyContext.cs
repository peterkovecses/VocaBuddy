namespace VocaBuddy.Infrastructure.Persistence;

public class VocaBuddyContext(DbContextOptions<VocaBuddyContext> options, AuditInterceptor auditInterceptor) : DbContext(options)
{
    private readonly AuditInterceptor _auditInterceptor = auditInterceptor;

    public DbSet<NativeWord> NativeWords { get; set; } = default!;
    public DbSet<ForeignWord> ForeignWords { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.AddInterceptors(_auditInterceptor);            
}
