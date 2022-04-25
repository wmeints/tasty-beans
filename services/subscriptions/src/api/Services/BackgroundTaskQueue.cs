using System.Threading.Channels;

namespace RecommendCoffee.Subscriptions.Api.Services;

public class BackgroundTaskQueue
{
    private readonly Channel<Func<CancellationToken, ValueTask>> _queue;

    public BackgroundTaskQueue()
    {
        _queue = Channel.CreateBounded<Func<CancellationToken, ValueTask>>(new BoundedChannelOptions(1000)
        {
            FullMode = BoundedChannelFullMode.Wait
        });
    }
    
    public async ValueTask Enqueue(Func<CancellationToken, ValueTask> task )
    {
        await _queue.Writer.WriteAsync(task);
    }

    public async ValueTask<Func<CancellationToken, ValueTask>> Dequeue()
    {
        return await _queue.Reader.ReadAsync();
    }
}