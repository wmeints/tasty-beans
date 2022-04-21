using System.Diagnostics.Metrics;

namespace RecommendCoffee.Templates.Microservice.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Subscriptions.Domain");
}