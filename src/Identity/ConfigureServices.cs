using Identity.Data;
using Identity.Interfaces;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity;

public static class ConfigureServices
{
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataProtection();

        services.AddDbContext<IdentityContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("IdentityDatabase")));

        var tokenValidationParametersConfigSection = configuration.GetSection("TokenValidationParameters");
        var tokenValidationParameters = new CustomTokenValidationParameters();
        tokenValidationParametersConfigSection.Bind(tokenValidationParameters);
        services.Configure<CustomTokenValidationParameters>(tokenValidationParametersConfigSection);

        services.AddScoped<IIdentityService, IdentityService>();

        var identityOptionsSection = configuration.GetSection("IdentityServer");
        var identityOptions = new IdentityOptions();
        identityOptionsSection.Bind(identityOptions);

        services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password = identityOptions.Password;
            options.User= identityOptions.User;
        })
        .AddEntityFrameworkStores<IdentityContext>()
        .AddDefaultTokenProviders();

        services.AddControllers();

        services.AddCors(options =>
        {
            options.AddPolicy(name: "_myAllowSpecificOrigins",
                              policy =>
                              {
                                  policy.WithOrigins("https://localhost:7095")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod();
                              });
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }
}
