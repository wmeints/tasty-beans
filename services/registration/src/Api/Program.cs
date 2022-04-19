using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using RecommendCoffee.Registration.Api;
using RecommendCoffee.Registration.Application.CommandHandlers;
using RecommendCoffee.Registration.Application.EventHandlers;
using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Payments;
using RecommendCoffee.Registration.Domain.Subscriptions;
using RecommendCoffee.Registration.Infrastructure.Agents;
using RecommendCoffee.Registration.Infrastructure.StateManagement;

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

builder.Services.AddHealthChecks()
    .AddUrlGroup(options =>
    {
        options.AddUri(
            new Uri("http://subscriptions/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );

        options.AddUri(
            new Uri("http://customermanagement/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );

        options.AddUri(
            new Uri("http://payments/healthz"),
            uo => uo.UseGet().UseTimeout(TimeSpan.FromSeconds(3))
        );
    });

builder.AddTelemetry("Registration",
    "RecommendCoffee.Registration.Api",
    "RecommendCoffee.Registration.Application",
    "RecommendCoffee.Registration.Domain",
    "RecommendCoffee.Registration.Infrastructure");

builder.Services.AddScoped<StartRegistrationCommandHandler>();

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<SubscriptionStartedEventHandler>();
builder.Services.AddScoped<PaymentMethodRegisteredEventHandler>();

builder.Services.AddSingleton<ISubscriptions, SubscriptionsAgent>();
builder.Services.AddSingleton<ICustomerManagement, CustomerManagementAgent>();
builder.Services.AddSingleton<IPayments, PaymentsAgent>();
builder.Services.AddSingleton<IStateStore, StateStore>();

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