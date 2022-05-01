using System.Diagnostics.Metrics;

namespace TastyBeans.Timer.Infrastructure;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("TastyBeans.Ratings.Infrastructure");
    public static readonly Counter<int> EventsPublished = Meter.CreateCounter<int>("ratings-events-published");
}