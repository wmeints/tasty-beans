using System.Diagnostics.Metrics;

namespace RecommendCoffee.Recommendations.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Recommendations.Domain");
}