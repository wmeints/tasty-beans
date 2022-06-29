using Microsoft.Extensions.DependencyInjection;
using TastyBeans.Shared.Application;

namespace TastyBeans.Shared.Infrastructure.EventBus;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddEventPublisher(this IServiceCollection services, Action<DaprEventPublisherOptions> configureOptions)
    {
        services.AddSingleton<IEventPublisher, DaprEventPublisher>();
        services.Configure(configureOptions);
        return services;
    }
}