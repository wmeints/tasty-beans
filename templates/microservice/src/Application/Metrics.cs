using System.Diagnostics.Metrics;

namespace RecommendCoffee.Templates.Microservice.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Templates.Microservice.Application");
}