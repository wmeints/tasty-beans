using Camunda.Api.Client;
using RecommendCoffee.TaskDispatcher;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context,services) =>
    {
        services.AddSingleton(provider => CamundaClient.Create(context.Configuration["TaskDispatcher:CamundaUrl"]));
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
