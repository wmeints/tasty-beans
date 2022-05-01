using System.Diagnostics.Metrics;

namespace TastyBeans.Timer.Application;

public static class Metrics
{
    private static readonly Meter Meter = new Meter("TastyBeans.Timer.Application");
    public static readonly Counter<int> MonthsPassed = Meter.CreateCounter<int>("timer-months-passed");
}