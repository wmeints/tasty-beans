using System.Diagnostics.Metrics;

namespace RecommendCoffee.Subscriptions.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Subscriptions.Infrastructure");

    private static Counter<int> _eventsPublished =
        _meter.CreateCounter<int>("subscriptions-events-published");
    
    public static Counter<int> EventsPublished => _eventsPublished;
}