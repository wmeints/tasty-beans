using System.Text.Json;
using System.Text.Json.Serialization;
using Dapr.Client;
using Microsoft.AspNetCore.HttpLogging;
using RecommendCoffee.Registration.Application.CommandHandlers;
using RecommendCoffee.Registration.Application.EventHandlers;
using RecommendCoffee.Registration.Domain.Common;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Subscriptions;
using RecommendCoffee.Registration.Infrastructure.Agents;
using RecommendCoffee.Registration.Infrastructure.StateManagement;

var builder = WebApplication.CreateBuilder(args);

var serializerSettings = new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
};

serializerSettings.Converters.Add(new JsonStringEnumConverter());

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

builder.Services.AddScoped<StartRegistrationCommandHandler>();

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<SubscriptionStartedEventHandler>();

builder.Services.AddSingleton<ISubscriptions, SubscriptionsAgent>();
builder.Services.AddSingleton<ICustomerManagement, CustomerManagementAgent>();
builder.Services.AddSingleton<IStateStore, StateStore>();

var app = builder.Build();

app.UseCloudEvents();

app.MapSubscribeHandler();
app.MapControllers();

app.Run();
