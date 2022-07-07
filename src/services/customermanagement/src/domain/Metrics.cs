using System.Diagnostics.Metrics;

namespace TastyBeans.CustomerManagement.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.CustomerManagement.Domain");
}