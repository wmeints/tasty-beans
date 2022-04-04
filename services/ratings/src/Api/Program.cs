using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Catalog.Infrastructure.EventBus;
using RecommendCoffee.Catalog.Infrastructure.Persistence;
using RecommendCoffee.Ratings.Application.Common;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
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

builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

    await dbContext.Database.MigrateAsync();
}

app.UseCloudEvents();

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
