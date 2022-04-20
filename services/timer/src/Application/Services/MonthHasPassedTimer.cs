using System.ComponentModel;
using Cronos;
using Microsoft.Extensions.Hosting;
using RecommendCoffee.Timer.Application.Common;
using RecommendCoffee.Timer.Domain.Events;
using Sgbj.Cron;

namespace RecommendCoffee.Timer.Application.Services;

public class MonthHasPassedTimer: BackgroundService
{
    private readonly string _timerExpression;
    private readonly IEventPublisher _eventPublisher;

    public MonthHasPassedTimer(string timerExpression, IEventPublisher eventPublisher)
    {
        _timerExpression = timerExpression;
        _eventPublisher = eventPublisher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new CronTimer(CronExpression.Parse(_timerExpression, CronFormat.IncludeSeconds));

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await _eventPublisher.PublishEventsAsync(new[] { new MonthHasPassedEvent() });
            Metrics.MonthsPassed.Add(1);
        }
    }
}