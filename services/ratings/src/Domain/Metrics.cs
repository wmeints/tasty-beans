using System.Diagnostics.Metrics;

namespace RecommendCoffee.Ratings.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Ratings.Domain");
}