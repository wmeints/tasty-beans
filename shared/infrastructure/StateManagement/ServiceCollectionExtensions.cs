using Microsoft.Extensions.DependencyInjection;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Infrastructure.StateManagement;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddStateStore(this IServiceCollection services) {
        services.AddSingleton<IStateStore, StateStore>();
        return services;
    }
}