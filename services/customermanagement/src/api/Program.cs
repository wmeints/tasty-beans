using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.CustomerManagement.Application.CommandHandlers;
using RecommendCoffee.CustomerManagement.Application.QueryHandlers;
using RecommendCoffee.CustomerManagement.Domain.Aggregates.CustomerAggregate;
using RecommendCoffee.CustomerManagement.Infrastructure.Persistence;
using RecommendCoffee.Shared.Diagnostics;
using RecommendCoffee.Shared.Infrastructure.EventBus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultDatabase"),
        opts=>opts.EnableRetryOnFailure());
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

        daprClientBuilder.UseJsonSerializationOptions(serializerOptions);
    });

builder.Services.AddHealthChecks().AddDbContextCheck<ApplicationDbContext>();

var telemetryOptions = builder.Configuration.GetSection("Telemetry").Get<TelemetryOptions>();

builder.Services.AddTracing(telemetryOptions,
    "RecommendCoffee.CustomerManagement.Api",
    "RecommendCoffee.CustomerManagement.Application",
    "RecommendCoffee.CustomerManagement.Domain",
    "RecommendCoffee.CustomerManagement.Infrastructure");

builder.Services.AddMetrics(telemetryOptions,
    "RecommendCoffee.CustomerManagement.Api",
    "RecommendCoffee.CustomerManagement.Application",
    "RecommendCoffee.CustomerManagement.Domain",
    "RecommendCoffee.CustomerManagement.Infrastructure");

builder.Services.AddEventPublisher(options => options.DeadLetterTopic = "customermanagement.deadletter.v1");
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<RegisterCustomerCommandHandler>();
builder.Services.AddScoped<FindCustomerQueryHandler>();
builder.Services.AddScoped<FindAllCustomersQueryHandler>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions()
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();