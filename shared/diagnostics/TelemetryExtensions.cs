using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RecommendCoffee.Shared.Diagnostics;

public static class TelemetryExtensions
{
    public static void AddMetrics(this IServiceCollection services, TelemetryOptions options, params string[] assemblyNames)
    {
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            options.Name,
            serviceVersion: options.Version,
            serviceInstanceId: machineName);

        services.AddOpenTelemetryMetrics(options =>
        {
            foreach (var assemblyName in assemblyNames)
            {
                options.AddMeter(assemblyName);
            }
            
            options
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddPrometheusExporter();
        });
    }

    public static void AddTracing(this IServiceCollection services, TelemetryOptions options, params string[] assemblyNames)
    {
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            options.Name,
            serviceVersion: options.Version,
            serviceInstanceId: machineName);

        services.AddSingleton(new ProxyGenerator());

        services.AddOpenTelemetryTracing(traceOptions =>
        {
            foreach (var assemblyName in assemblyNames)
            {
                traceOptions.AddSource(assemblyName);
            }

            traceOptions
                .SetResourceBuilder(resourceBuilder)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.Filter = (httpContext) => httpContext.Request.Path != "/healthz";
                })
                .AddSqlClientInstrumentation()
                .AddJaegerExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(options.Endpoint);
                    exporterOptions.Protocol = JaegerExportProtocol.HttpBinaryThrift;
                });
        });
    }
}