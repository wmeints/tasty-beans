using System.Diagnostics.Metrics;

namespace RecommendCoffee.Ratings.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Ratings.Infrastructure");
    private static Counter<int> _eventsPublished = _meter.CreateCounter<int>("ratings-events-published");

    public static Counter<int> EventsPublished => _eventsPublished;
}