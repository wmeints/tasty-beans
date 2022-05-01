using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TastyBeans.Registration.Application.CommandHandlers;
using TastyBeans.Registration.Application.EventHandlers;
using TastyBeans.Registration.Domain.Customers;
using TastyBeans.Registration.Domain.Payments;
using TastyBeans.Registration.Domain.Subscriptions;
using TastyBeans.Registration.Infrastructure.Agents;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.StateManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    })
    .AddDapr(daprClientBuilder =>
    {
        var serializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        serializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddHeaderPropagation();

builder.Services.AddHealthChecks()
    .AddUrlGroup(options =>
    {
        options.AddUri(
            new Uri(new Uri(builder.Configuration["ServiceLocations:Subscriptions"]), "/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );

        options.AddUri(
            new Uri(new Uri(builder.Configuration["ServiceLocations:CustomerManagement"]), "/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );

        options.AddUri(
            new Uri(new Uri(builder.Configuration["ServiceLocations:Payments"]), "/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );
    });

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Registration.Api",
    "TastyBeans.Registration.Application",
    "TastyBeans.Registration.Domain",
    "TastyBeans.Registration.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Registration.Api",
    "TastyBeans.Registration.Application",
    "TastyBeans.Registration.Domain",
    "TastyBeans.Registration.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddScoped<StartRegistrationCommandHandler>();

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<SubscriptionStartedEventHandler>();
builder.Services.AddScoped<PaymentMethodRegisteredEventHandler>();

builder.Services.AddSingleton<ISubscriptions, SubscriptionsAgent>();
builder.Services.AddSingleton<ICustomerManagement, CustomerManagementAgent>();
builder.Services.AddSingleton<IPayments, PaymentsAgent>();
builder.Services.AddStateStore();

var app = builder.Build();

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();