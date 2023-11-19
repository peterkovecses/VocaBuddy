using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Infrastructure.Interfaces;
using VocaBuddy.Infrastructure.Persistence;
using VocaBuddy.Infrastructure.Persistence.Interceptors;
using VocaBuddy.Infrastructure.Services;

namespace VocaBuddy.Infrastructure;

public static class ConfigureServices
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
