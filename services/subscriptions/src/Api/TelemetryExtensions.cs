using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RecommendCoffee.Subscriptions.Api;

public static class TelemetryExtensions
{
    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var serviceName = "Subscriptions";
        var serviceVersion = Environment.GetEnvironmentVariable("IMAGE_TAG") ?? "0.0.0.0";
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: machineName);
        
        builder.Services.AddOpenTelemetryTracing(options =>
        {
            options
                .AddSource(serviceName)
                .SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
                })
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(options =>
                {
                    options.Filter = (httpContext) => httpContext.Request.Path != "/healthz";
                })
                .AddSqlClientInstrumentation();
        });

        builder.Services.AddOpenTelemetryMetrics(options =>
        {
            options
                .SetResourceBuilder(resourceBuilder)
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation();
        });

        builder.Logging.AddOpenTelemetry(options =>
        {
            options.SetResourceBuilder(resourceBuilder);
            options.AddOtlpExporter(exporterOptions =>
            {
                exporterOptions.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
            });
        });

        builder.Services.Configure<OpenTelemetryLoggerOptions>(options =>
        {
            options.IncludeScopes = true;
            options.ParseStateValues = true;
            options.IncludeFormattedMessage = true;
        });
    }
}