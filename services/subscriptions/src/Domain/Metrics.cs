using System.Diagnostics.Metrics;

namespace RecommendCoffee.Subscriptions.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Subscriptions.Domain");
}