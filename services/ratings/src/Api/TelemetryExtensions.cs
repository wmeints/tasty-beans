using OpenTelemetry.Exporter;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RecommendCoffee.Ratings.Api;

public static class TelemetryExtensions
{
    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var serviceName = "ratings.default";
        var serviceVersion = Environment.GetEnvironmentVariable("IMAGE_TAG") ?? "0.0.0.0";
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: machineName);
        
        builder.Services.AddOpenTelemetryTracing(options =>
        {
            options
                .AddSource("RecommendCoffee.Ratings.Domain")
                .AddSource("RecommendCoffee.Ratings.Application")
                .AddSource("RecommendCoffee.Ratings.Infrastructure")
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
            options
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddMeter("RecommendCoffee.Ratings.Api")
                .AddMeter("RecommendCoffee.Ratings.Application")
                .AddMeter("RecommendCoffee.Ratings.Domain")
                .AddMeter("RecommendCoffee.Ratings.Infrastructure")
                .AddPrometheusExporter();
        });
    }
}