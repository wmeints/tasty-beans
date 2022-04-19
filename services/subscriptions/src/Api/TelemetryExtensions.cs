using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RecommendCoffee.Subscriptions.Api;

public static class TelemetryExtensions
{
    public static void AddTelemetry(this WebApplicationBuilder builder, string serviceName, params string[] assemblyNames)
    {
        Sdk.SetDefaultTextMapPropagator(new B3Propagator());

        var podNamespace = Environment.GetEnvironmentVariable("POD_NAMESPACE");
        var serviceIdentity = $"{serviceName.ToLower()}.{podNamespace}";
        var serviceVersion = Environment.GetEnvironmentVariable("IMAGE_TAG") ?? "0.0.0.0";
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceIdentity,
            serviceVersion: serviceVersion,
            serviceInstanceId: machineName);
        
        builder.Services.AddOpenTelemetryTracing(options =>
        {
            foreach (var assemblyName in assemblyNames)
            {
                options.AddSource(assemblyName);
            }
            
            options
                .SetResourceBuilder(resourceBuilder)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.Filter = (httpContext) => httpContext.Request.Path != "/healthz";
                })
                .AddSqlClientInstrumentation()
                .AddJaegerExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(builder.Configuration["Telemetry:Spans"]);
                    exporterOptions.Protocol = JaegerExportProtocol.HttpBinaryThrift;
                });
        });

        builder.Services.AddOpenTelemetryMetrics(options =>
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
}