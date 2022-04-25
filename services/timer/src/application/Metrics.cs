using System.Diagnostics.Metrics;

namespace RecommendCoffee.Timer.Application;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("RecommendCoffee.Timer.Application");
    public static readonly Counter<int> MonthsPassed = Meter.CreateCounter<int>("timer-months-passed");
}