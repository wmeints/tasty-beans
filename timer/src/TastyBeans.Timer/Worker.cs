using Jasper;
using NodaTime;
using TastyBeans.Timer.Events;

namespace TastyBeans.Timer;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IMessagePublisher _messagePublisher;

    public Worker(ILogger<Worker> logger, IMessagePublisher messagePublisher)
    {
        _logger = logger;
        _messagePublisher = messagePublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var startDate = LocalDate.FromDateTime(DateTime.UtcNow);
        startDate = DateAdjusters.DayOfMonth(1)(startDate);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var dateTime = startDate.ToDateTimeUnspecified();
            
            _logger.LogInformation("Month has passed: {time}", dateTime);
            await _messagePublisher.PublishAsync(new MontHasPassed(dateTime));

            startDate = startDate.PlusMonths(1);
            
            await Task.Delay(10000, stoppingToken);
        }
    }
}
