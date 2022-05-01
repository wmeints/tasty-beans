using TastyBeans.Subscriptions.Application.EventHandlers;

namespace TastyBeans.Subscriptions.Api.Services;

public class BackgroundTaskService: BackgroundService
{
    private readonly ILogger<BackgroundTaskService> _logger;
    private readonly BackgroundTaskQueue _backgroundTaskQueue;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundTaskService(BackgroundTaskQueue backgroundTaskQueue, IServiceProvider serviceProvider, ILogger<BackgroundTaskService> logger)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background task processor");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Handling month-has-passed event in background queue");

                var workItem = await _backgroundTaskQueue.Dequeue();

                await using (var scope = _serviceProvider.CreateAsyncScope())
                {
                    var eventHandler = scope.ServiceProvider.GetRequiredService<MonthHasPassedEventHandler>();
                    await eventHandler.HandleAsync(workItem);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to process background task");
            }
        }
        
        _logger.LogInformation("Stopping background task processor");
    }
}