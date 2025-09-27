namespace Identity.Application.Contracts;

public interface IIdentityContext
{
    DbSet<CustomRefreshToken> RefreshTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}