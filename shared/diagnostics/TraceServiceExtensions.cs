using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace RecommendCoffee.Shared.Diagnostics;

public static class TraceServiceExtensions
{
    public static void AddTracing<TInterface>(this IServiceCollection services) where TInterface : class
    {
        services.Decorate<TInterface>((inner, serviceProvider) =>
        {
            var proxyGenerator = serviceProvider.GetRequiredService<ProxyGenerator>();
            var activitySourceRegistry = serviceProvider.GetRequiredService<ActivitySourceRegistry>();

            var proxy = typeof(TInterface).IsInterface switch
            {
                true => proxyGenerator.CreateInterfaceProxyWithTarget(
                    inner, new TracingInterceptor(activitySourceRegistry)),
                false => proxyGenerator.CreateClassProxyWithTarget(
                    inner, new TracingInterceptor(activitySourceRegistry))
            };

            return proxy;
        });
    }
}