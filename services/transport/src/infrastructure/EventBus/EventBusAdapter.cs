using Akka.Actor;
using Akka.Event;
using Microsoft.Extensions.DependencyInjection;
using RecommendCoffee.Shared.Application;
using RecommendCoffee.Shared.Domain;

namespace RecommendCoffee.Transport.Infrastructure.EventBus;

public class EventBusAdapter : ReceiveActor
{
    private readonly IEventPublisher _eventPublisher;

    public EventBusAdapter(IEventPublisher eventPublisher)
    {
        _eventPublisher = eventPublisher;

        Context.GetLogger().Info("Starting event bus adapter for domain events");
        Context.System.EventStream.Subscribe(Self, typeof(IDomainEvent));

        ReceiveAsync<IDomainEvent>(OnReceiveDomainEvent);
    }

    protected override void PostStop()
    {
        base.PostStop();
        
        Context.GetLogger().Info("Stopped event bus adapter");
    }

    private async Task OnReceiveDomainEvent(IDomainEvent obj)
    {
        await _eventPublisher.PublishEventsAsync(new[] { obj });
    }

    public static Props Props(IEventPublisher eventPublisher)
    {
        return new Props(
            type: typeof(EventBusAdapter),
            supervisorStrategy: new OneForOneStrategy(ex => Directive.Restart),
            args: eventPublisher);
    }
}
