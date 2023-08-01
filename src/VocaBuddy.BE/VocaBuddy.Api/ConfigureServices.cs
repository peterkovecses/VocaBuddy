namespace VocaBuddy.Api;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
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

        return services;
    }
}
