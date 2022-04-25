using System.Diagnostics.Metrics;

namespace RecommendCoffee.Timer.Infrastructure;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("RecommendCoffee.Ratings.Infrastructure");
    public static readonly Counter<int> EventsPublished = Meter.CreateCounter<int>("ratings-events-published");
}