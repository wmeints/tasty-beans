using RecommendCoffee.Registration.Application.CommandHandlers;
using RecommendCoffee.Registration.Application.Common;
using RecommendCoffee.Registration.Application.EventHandlers;
using RecommendCoffee.Registration.Domain.Customers;
using RecommendCoffee.Registration.Domain.Subscriptions;
using RecommendCoffee.Registration.Infrastructure.Agents;
using RecommendCoffee.Registration.Infrastructure.StateManagement;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDaprClient();
builder.Services.AddControllers().AddDapr();
builder.Services.AddScoped<StartRegistrationCommandHandler>();

builder.Services.AddScoped<CustomerRegisteredEventHandler>();
builder.Services.AddScoped<SubscriptionStartedEventHandler>();

builder.Services.AddSingleton<ISubscriptions, SubscriptionsAgent>();
builder.Services.AddSingleton<ICustomerManagement, CustomerManagementAgent>();
builder.Services.AddSingleton<IStateStore, StateStore>();

var app = builder.Build();
app.MapControllers();
app.Run();
