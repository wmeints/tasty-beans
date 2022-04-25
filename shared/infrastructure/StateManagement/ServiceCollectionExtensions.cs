using Microsoft.Extensions.DependencyInjection;
using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Shared.Infrastructure.StateManagement;

public static class ServiceCollectionExtensions {
    public static IServiceCollection AddStateStore(this IServiceCollection services) {
        services.AddSingleton<IStateStore, StateStore>();
        return services;
    }
}