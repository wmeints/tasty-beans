using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Templates.Microservice.Api;
using RecommendCoffee.Templates.Microservice.Application.Common;
using RecommendCoffee.Templates.Microservice.Infrastructure.EventBus;
using RecommendCoffee.Templates.Microservice.Infrastructure.Persistence;

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

builder.Services.AddHealthChecks()
    .AddSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"))
    .AddDbContextCheck<ApplicationDbContext>();

builder.AddTelemetry("Shipping",
    "RecommendCoffee.Templates.Microservice.Api",
    "RecommendCoffee.Templates.Microservice.Application",
    "RecommendCoffee.Templates.Microservice.Domain",
    "RecommendCoffee.Templates.Microservice.Infrastructure");

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();    
}

app.UseOpenTelemetryPrometheusScrapingEndpoint();

app.UseCloudEvents();

app.MapHealthChecks("/healthz", new HealthCheckOptions 
{
    AllowCachingResponses = false
});

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
