using System.Diagnostics.Metrics;

namespace RecommendCoffee.Identity.Api;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Identity.Application");
}