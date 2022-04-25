using System.Diagnostics.Metrics;

namespace RecommendCoffee.Catalog.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Catalog.Infrastructure");
    private static Counter<int> _eventsPublishedCounter = _meter.CreateCounter<int>("catalog-events-published");

    public static Counter<int> EventsPublished => _eventsPublishedCounter;
}