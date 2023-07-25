using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using VocaBuddy.Application.Interfaces;
using VocaBuddy.Application.Mappings;

namespace VocaBuddy.Application;

public static class ConfigureServices
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        IMapper mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);

        services.AddMediatR(config => config.RegisterServicesFromAssembly(ApplicationAssemblyMarker.Assembly));

        return services;
    }
}
