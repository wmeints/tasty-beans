using System.Diagnostics.Metrics;

namespace RecommendCoffee.Ratings.Domain;

public class Metrics
{
    private static Meter Meter = new Meter("RecommendCoffee.Ratings.Domain");
}