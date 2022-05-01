using System.Diagnostics.Metrics;

namespace TastyBeans.Registration.Application;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.Registration.Application");

    public static Counter<int> RegistrationsStarted { get; } = _meter.CreateCounter<int>("registration-registrations-started");

    public static Counter<int> RegistrationsCompleted { get; } = _meter.CreateCounter<int>("registration-registrations-completed");
}