namespace EmailDispatcher;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SmtpSettings>(configuration.GetSection("SmtpSettings"));
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
        
        services.AddSerilog(loggerConfiguration =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                // .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter());
                .WriteTo.Console();
        });
        
        services.AddEasyNetQ("host=localhost").UseSystemTextJson(serializerOptions);
        services.AddSingleton<IEmailSender, EmailSender>();
        services.AddHostedService<UserRegisteredWorker>();
        services.AddMediatR(config => config.RegisterServicesFromAssembly(AssemblyMarker.Assembly));
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

        return services;
    }
}