using System.Diagnostics.Metrics;

namespace RecommendCoffee.Ratings.Application;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("RecommendCoffee.Ratings.Application");
    public static readonly Counter<int> RatingsRegistered = Meter.CreateCounter<int>("ratings-ratings-registered");
}