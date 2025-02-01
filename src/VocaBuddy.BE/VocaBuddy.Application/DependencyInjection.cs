namespace VocaBuddy.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddMediatR(config => config.RegisterServicesFromAssembly(ApplicationAssemblyMarker.Assembly));
        services.AddValidatorsFromAssembly(ApplicationAssemblyMarker.Assembly);
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));
        services.AddScoped<IPipelineBehavior<GetNativeWordByIdQuery, Result<NativeWordDto>>, GetNativeWordUserIdMatchBehavior>();
        services.AddScoped<IPipelineBehavior<GetRandomNativeWordsQuery, Result<List<NativeWordDto>>>, GetRandomNativeWordsWordCountCheckBehavior>();
        services.AddScoped<IPipelineBehavior<GetLatestNativeWordsQuery, Result<List<NativeWordDto>>>, GetLatestNativeWordsWordCountCheckBehavior>();

        return services;
    }
}
