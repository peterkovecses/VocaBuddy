using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using VocaBuddy.Application.Mappings;
using VocaBuddy.Application.PipelineBehaviors;
using VocaBuddy.Application.Queries;
using VocaBuddy.Shared.Dtos;
using VocaBuddy.Shared.Models;

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
        services.AddValidatorsFromAssembly(ApplicationAssemblyMarker.Assembly);
        services.AddScoped<IPipelineBehavior<GetNativeWordByIdQuery, Result<NativeWordDto>>, GetNativeWordUserIdMatchBehavior>();
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(LoggingBehavior<,>));
        services.AddScoped(
            typeof(IPipelineBehavior<,>),
            typeof(ValidationBehavior<,>));

        return services;
    }
}
