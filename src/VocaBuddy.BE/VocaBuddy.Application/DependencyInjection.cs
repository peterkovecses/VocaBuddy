using VocaBuddy.Application.Features.NativeWord.Queries.GetById;
using VocaBuddy.Application.Features.NativeWord.Queries.GetLatest;
using VocaBuddy.Application.Features.NativeWord.Queries.GetRandom;
using VocaBuddy.Application.Logging;
using VocaBuddy.Application.Validation;

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
        services.AddScoped<IPipelineBehavior<GetNativeWordByIdQuery, Result<CompactNativeWordDto>>, GetNativeWordUserIdMatchBehavior>();
        services.AddScoped<IPipelineBehavior<GetRandomNativeWordsQuery, Result<List<NativeWordDto>>>, GetRandomNativeWordsWordCountCheckBehavior>();
        services.AddScoped<IPipelineBehavior<GetLatestNativeWordsQuery, Result<List<NativeWordDto>>>, GetLatestNativeWordsWordCountCheckBehavior>();

        return services;
    }
}
