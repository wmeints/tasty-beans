using System.Diagnostics.Metrics;

namespace RecommendCoffee.Ratings.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Ratings.Application");
    private static Counter<int> _ratingsRegistered = _meter.CreateCounter<int>("ratings-ratings-registered");

    public static Counter<int> RatingsRegistered => _ratingsRegistered;
}