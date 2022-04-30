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
    private readonly BackgroundTaskQueue _backgroundTaskQueue;
    
    public EventsController(BackgroundTaskQueue backgroundTaskQueue)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
    }

    [HttpPost("MonthHasPassed")]
    [Topic("pubsub", "timer.month-has-passed.v1")]
    public async Task<IActionResult> OnMonthHasPassed(MonthHasPassedEvent evt)
    {
        // Queue the event on the background worker for processing.
        await _backgroundTaskQueue.Enqueue(evt);
        
        return Accepted();
    }
}