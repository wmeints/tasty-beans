using System.Diagnostics.Metrics;

namespace RecommendCoffee.Recommendations.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Recommendations.Application");

}