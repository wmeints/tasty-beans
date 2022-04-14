using System.Diagnostics.Metrics;

namespace RecommendCoffee.Payments.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Payments.Infrastructure");
    private static Counter<int> _eventsPublished = _meter.CreateCounter<int>("payments-events-published");

    public static Counter<int> EventsPublished => _eventsPublished;
}