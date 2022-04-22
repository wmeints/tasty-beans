namespace RecommendCoffee.Subscriptions.Api.Services;

public class BackgroundTaskService: BackgroundService
{
    private readonly ILogger<BackgroundTaskService> _logger;
    private readonly BackgroundTaskQueue _backgroundTaskQueue;

    public BackgroundTaskService(BackgroundTaskQueue backgroundTaskQueue, ILogger<BackgroundTaskService> logger)
    {
        _backgroundTaskQueue = backgroundTaskQueue;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Starting background task processor");
        
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var workItem = await _backgroundTaskQueue.Dequeue();
                await workItem(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Failed to process background task");
            }
        }
        
        _logger.LogInformation("Stopping background task processor");
    }
}