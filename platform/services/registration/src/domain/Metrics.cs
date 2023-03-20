using System.Diagnostics.Metrics;

namespace TastyBeans.Registration.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.Registration.Domain");
}