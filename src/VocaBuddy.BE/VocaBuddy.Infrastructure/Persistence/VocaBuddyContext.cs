using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace VocaBuddy.Infrastructure.Persistence;

public class VocaBuddyContext : DbContext
{
    private readonly AuditInterceptor _auditInterceptor;

    public VocaBuddyContext(DbContextOptions<VocaBuddyContext> options, AuditInterceptor auditInterceptor) 
        : base(options)
    {
        _auditInterceptor = auditInterceptor;
    }

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
