using System.Diagnostics.Metrics;

namespace RecommendCoffee.Registration.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Registration.Application");

    private static Counter<int> _registrationsStarted = _meter.CreateCounter<int>("registration-registrations-started");
    private static Counter<int> _registrationsCompleted = _meter.CreateCounter<int>("registration-registrations-completed");

    public static Counter<int> RegistrationsStarted => _registrationsStarted;
    public static Counter<int> RegistrationsCompleted => _registrationsCompleted;
}