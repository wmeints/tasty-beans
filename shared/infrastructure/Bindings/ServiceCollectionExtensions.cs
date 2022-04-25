using Microsoft.Extensions.DependencyInjection;
using RecommendCoffee.Shared.Application;

namespace RecommendCoffee.Shared.Infrastructure.Bindings;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
        return services;
    }
}