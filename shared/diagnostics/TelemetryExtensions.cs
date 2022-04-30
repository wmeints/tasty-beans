using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
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

        // Make sure to map the B3 headers as this is what istio is using at the moment for tracing.
        // You can read more about B3 headers here: https://github.com/openzipkin/b3-propagation
        Sdk.SetDefaultTextMapPropagator(new B3Propagator());

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
                    exporterOptions.Endpoint = new Uri(options.TracingEndpoint);
                    exporterOptions.Protocol = JaegerExportProtocol.HttpBinaryThrift;
                });
        });
    }

    public static void AddLogging(this IServiceCollection services, TelemetryOptions options)
    {
        services.AddLogging(builder =>
        {
            builder.AddSeq(options.LoggingEndpoint);
        });
    }
}