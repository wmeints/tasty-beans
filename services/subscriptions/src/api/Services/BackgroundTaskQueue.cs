using System.Threading.Channels;
using TastyBeans.Subscriptions.Application.IntegrationEvents;

namespace TastyBeans.Subscriptions.Api.Services;

public class BackgroundTaskQueue
{
    private readonly Channel<MonthHasPassedEvent> _queue;

    public BackgroundTaskQueue()
    {
        _queue = Channel.CreateBounded<MonthHasPassedEvent>(new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }
    
    public async ValueTask Enqueue(MonthHasPassedEvent evt)
    {
        await _queue.Writer.WriteAsync(evt);
    }

    public async ValueTask<MonthHasPassedEvent> Dequeue()
    {
        return await _queue.Reader.ReadAsync();
    }
}