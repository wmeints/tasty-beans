using System.Diagnostics.Metrics;

namespace RecommendCoffee.CustomerManagement.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.CustomerManagement.Infrastructure");
    private static Counter<int> _eventsPublished = _meter.CreateCounter<int>("customermanagement-events-published");

    public static Counter<int> EventsPublished => _eventsPublished;
}