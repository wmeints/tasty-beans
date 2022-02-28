using Microsoft.EntityFrameworkCore;
using RecommendCoffee.Subscriptions.Application.CommandHandlers;
using RecommendCoffee.Subscriptions.Application.Common;
using RecommendCoffee.Subscriptions.Application.QueryHandlers;
using RecommendCoffee.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using RecommendCoffee.Subscriptions.Infrastructure.DomainEvents;
using RecommendCoffee.Subscriptions.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultDatabase"));
});

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();
builder.Services.AddSingleton<IEventPublisher, DaprEventPublisher>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<StartSubscriptionCommandHandler>();
builder.Services.AddScoped<CancelSubscriptionCommandHandler>();
builder.Services.AddScoped<ChangeShippingFrequencyCommandHandler>();
builder.Services.AddScoped<FindSubscriptionQueryHandler>();

var app = builder.Build();

await using var scope = app.Services.CreateAsyncScope();
await using var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

await dbContext.Database.MigrateAsync();

app.MapControllers();

app.Run();
