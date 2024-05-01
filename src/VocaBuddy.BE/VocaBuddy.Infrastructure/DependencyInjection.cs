using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace VocaBuddy.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<VocaBuddyContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("VocaBuddyDatabase")));

        services.AddScoped<IDateTimeProvider, DateTimeProvider>();
        services.AddScoped<AuditInterceptor>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
