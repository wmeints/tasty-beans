using Jasper;
using Jasper.RabbitMQ;
using TastyBeans.Timer;
using TastyBeans.Timer.Events;

var host = Host.CreateDefaultBuilder(args)
    .UseJasper((context, options) =>
    {
        options
            .UseRabbitMq(rabbit =>
            {
                rabbit.HostName = context.Configuration["EventBus:HostName"];
                rabbit.UserName = context.Configuration["EventBus:UserName"];
                rabbit.Password = context.Configuration["EventBus:Password"];
                rabbit.Port = Int32.Parse(context.Configuration["EventBus:Port"]);
            });

        options.PublishMessage<MontHasPassed>().ToRabbit("timer.month-has-passed.v1", "events");

        options.Services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
