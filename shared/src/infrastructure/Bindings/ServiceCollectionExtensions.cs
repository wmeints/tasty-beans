using Microsoft.Extensions.DependencyInjection;
using TastyBeans.Shared.Application;

namespace TastyBeans.Shared.Infrastructure.Bindings;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services.AddSingleton<IEmailSender, EmailSender>();
        return services;
    }
}