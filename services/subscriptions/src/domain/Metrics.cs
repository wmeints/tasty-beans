using System.Diagnostics.Metrics;

namespace TastyBeans.Subscriptions.Domain;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.Subscriptions.Domain");

    public static Counter<int> ShipmentsCreated = _meter.CreateCounter<int>("subscriptions-shipments-created");
}