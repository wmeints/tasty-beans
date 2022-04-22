using Dapr;
using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Subscriptions.Api.Services;
using RecommendCoffee.Subscriptions.Application.EventHandlers;
using RecommendCoffee.Subscriptions.Application.IntegrationEvents;

namespace RecommendCoffee.Subscriptions.Api.Controllers;

[ApiController]
[Route("/events")]
public class EventsController: ControllerBase
{
    private readonly MonthHasPassedEventHandler _monthHasPassedEventHandler;
    private readonly BackgroundTaskQueue _backgroundTaskQueue;
    
    public EventsController(MonthHasPassedEventHandler monthHasPassedEventHandler, BackgroundTaskQueue backgroundTaskQueue)
    {
        _monthHasPassedEventHandler = monthHasPassedEventHandler;
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    [HttpPost("MonthHasPassed")]
    [Topic("pubsub", "timer.month-has-passed.v1")]
    public async Task<IActionResult> OnMonthHasPassed(MonthHasPassedEvent evt)
    {
        // Queue the actual operation on the background worker otherwise this HTTP call might take a bit too long.
        await _backgroundTaskQueue.Enqueue(async cancellationToken => 
            await _monthHasPassedEventHandler.HandleAsync(evt));
        
        return Accepted();
    }
}