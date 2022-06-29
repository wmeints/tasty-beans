using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TastyBeans.Shared.Application;

namespace TastyBeans.Shared.Infrastructure.EventStore;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEventStore(this IServiceCollection services,
        Action<DbContextOptionsBuilder> configureStorage)
    {
        services.AddDbContext<EventStoreDbContext>(configureStorage);
        services.AddScoped<IEventStore, EventStore>();

        return services;
    }
}