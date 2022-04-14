using System.Diagnostics.Metrics;

namespace RecommendCoffee.Identity.Infrastructure;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Identity.Infrastructure");
}