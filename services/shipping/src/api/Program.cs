using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Shipping.Infrastructure.Persistence;
using TastyBeans.Shared.Diagnostics;
using TastyBeans.Shared.Infrastructure.EventBus;
using TastyBeans.Shipping.Application.CommandHandlers;
using TastyBeans.Shipping.Application.EventHandlers;
using TastyBeans.Shipping.Application.Services;
using TastyBeans.Shipping.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shipping.Domain.Aggregates.ShippingOrderAggregate;
using TastyBeans.Shipping.Infrastructure.Agents;
using TastyBeans.Shipping.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase"),
        opts => opts.EnableRetryOnFailure());
});

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
builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "TastyBeans.Shipping.Api",
    "TastyBeans.Shipping.Application",
    "TastyBeans.Shipping.Domain",
    "TastyBeans.Shipping.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "TastyBeans.Shipping.Api",
    "TastyBeans.Shipping.Application",
    "TastyBeans.Shipping.Domain",
    "TastyBeans.Shipping.Infrastructure");

builder.Services.AddLogging(telemetryOptions);

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "shipping.deadletter.v1");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IShippingOrderRepository, ShippingOrderRepository>();
builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<ProductRegisteredEventHandler>();
builder.Services.AddScoped<ProductUpdatedEventHandler>();
builder.Services.AddScoped<CreateShippingOrderCommandHandler>();

builder.Services.AddHttpClient<ITransportCompany, TransportCompanyAgent>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceLocations:Transport"]);
});

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.UseOpenTelemetryPrometheusScrapingEndpoint();
app.UseHeaderPropagation();
app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
