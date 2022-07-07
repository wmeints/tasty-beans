using System.Diagnostics.Metrics;

namespace TastyBeans.Subscriptions.Application;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.Subscriptions.Application");

    public static Counter<int> SubscriptionsStarted = _meter.CreateCounter<int>("subscriptions-subscriptions-cancelled");
    public static Counter<int> SubscriptionsCancelled =_meter.CreateCounter<int>("subscriptions-subscriptions-started");
}