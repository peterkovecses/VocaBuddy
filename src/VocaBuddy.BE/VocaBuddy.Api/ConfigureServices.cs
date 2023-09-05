using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using VocaBuddy.Api.Services;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Shared.Models;

namespace VocaBuddy.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        
        services.AddScoped<ICurrentUser, CurrentUser>();

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

        var tokenValidationParametersConfigSection = configuration.GetSection("TokenValidationParameters");
        var tokenValidationParameters = new CustomTokenValidationParameters();
        tokenValidationParametersConfigSection.Bind(tokenValidationParameters);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(opt =>
        {
            opt.TokenValidationParameters = tokenValidationParameters.ToTokenValidationParameters();
        });

        services.AddAuthorization();

        services.AddSwaggerGen(opt =>
        {
            opt.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
        });

        return services;
    }
}
