using System.Diagnostics.Metrics;

namespace RecommendCoffee.Recommendations.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Recommendations.Infrastructure");

    private static Counter<int> _eventsPublished =
        _meter.CreateCounter<int>("recommendations-events-published");
    
    public static Counter<int> EventsPublished => _eventsPublished;
}