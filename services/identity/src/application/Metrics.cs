using System.Diagnostics.Metrics;

namespace RecommendCoffee.Identity.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Identity.Application");
}