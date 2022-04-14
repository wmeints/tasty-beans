﻿using System.Diagnostics.Metrics;
using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.AspNetCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace RecommendCoffee.Catalog.Api;

public static class TelemetryExtensions
{
    public static void AddTelemetry(this WebApplicationBuilder builder)
    {
        var serviceName = "Catalog";
        var serviceVersion = Environment.GetEnvironmentVariable("IMAGE_TAG") ?? "0.0.0.0";
        var machineName = Environment.MachineName;

        var resourceBuilder = ResourceBuilder.CreateDefault().AddService(
            serviceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: machineName);
        
        builder.Services.AddOpenTelemetryTracing(options =>
        {
            options
                .AddSource("RecommendCoffee.Catalog")
                .SetResourceBuilder(resourceBuilder)
                .AddOtlpExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
                })
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation(instrumentationOptions =>
                {
                    instrumentationOptions.Filter = (httpContext) => httpContext.Request.Path != "/healthz";
                })
                .AddSqlClientInstrumentation();
        });

        builder.Services.AddOpenTelemetryMetrics(options =>
        {
            options
                .SetResourceBuilder(resourceBuilder)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddAspNetCoreInstrumentation()
                .AddMeter("RecommendCoffee.Catalog.Api")
                .AddMeter("RecommendCoffee.Catalog.Application")
                .AddMeter("RecommendCoffee.Catalog.Domain")
                .AddMeter("RecommendCoffee.Catalog.Infrastructure")
                .AddOtlpExporter(exporterOptions =>
                {
                    exporterOptions.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]);
                });
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