using Cronos;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sgbj.Cron;
using TastyBeans.Shared.Application;
using TastyBeans.Timer.Domain.Events;

namespace TastyBeans.Timer.Application.Services;

public class MonthHasPassedTimer : BackgroundService
{
    private readonly string _timerExpression;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<MonthHasPassedTimer> _logger;

    public MonthHasPassedTimer(string timerExpression, IEventPublisher eventPublisher,
        ILogger<MonthHasPassedTimer> logger)
    {
        _timerExpression = timerExpression;
        _eventPublisher = eventPublisher;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new CronTimer(CronExpression.Parse(_timerExpression, CronFormat.IncludeSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            _logger.LogInformation("Month has passed");
            
            await _eventPublisher.PublishEventsAsync(new[] { new MonthHasPassedEvent() });
            Metrics.MonthsPassed.Add(1);
        }
    }
}