using Camunda.Api.Client;
using Camunda.Api.Client.ExternalTask;

namespace RecommendCoffee.TaskDispatcher;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly CamundaClient _camundaClient;
    private readonly IEnumerable<ExternalTaskHandler> _externalTaskHandlers;

    public Worker(
        IEnumerable<ExternalTaskHandler> externalTaskHandlers, 
        CamundaClient camundaClient,
        ILogger<Worker> logger)
    {
        _logger = logger;
        _camundaClient = camundaClient;
        _externalTaskHandlers = externalTaskHandlers;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topicNames = _externalTaskHandlers
            .Select(x => new FetchExternalTaskTopic(x.TopicName, 30_000))
            .ToList();
        
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogDebug("Retrieving external tasks");
            
            var tasks = await _camundaClient.ExternalTasks.FetchAndLock(new FetchExternalTasks
            {
                Topics =topicNames,
                AsyncResponseTimeout = 10_000,
                WorkerId = "task-dispatcher"
            });

            if (tasks.Count > 0)
            {
                _logger.LogInformation("Handling {TaskCount} tasks", tasks.Count);    
            }
            
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = 4
            };

            await Parallel.ForEachAsync(tasks, parallelOptions, async (task, token) =>
            {
                var handler = _externalTaskHandlers.SingleOrDefault(
                    x => x.TopicName == task.TopicName);

                if (handler != null)
                {
                    try
                    {
                        _logger.LogDebug(
                            "Invoke task handler {TopicName} for task {ExternalTaskId}",
                            task.TopicName, task.Id
                        );

                        var outputVariables = await handler.ExecuteAsync(task.Variables);

                        _logger.LogDebug("Completing task {ExternalTaskId}", task.Id);

                        await _camundaClient.ExternalTasks[task.Id].Complete(new CompleteExternalTask
                        {
                            Variables = outputVariables,
                            WorkerId = "task-dispatcher"
                        });
                    }
                    catch (Exception ex)
                    {
                        await _camundaClient.ExternalTasks[task.Id].HandleFailure(new ExternalTaskFailure
                        {
                            ErrorMessage = ex.Message
                        });
                    }
                }
                else
                {
                    _logger.LogWarning(
                        "No handler registered for topic {TopicName}", 
                        task.TopicName
                    );
                }
            });
        }
    }
}