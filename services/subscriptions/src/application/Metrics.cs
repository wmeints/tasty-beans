using System.Diagnostics.Metrics;

namespace RecommendCoffee.Subscriptions.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Subscriptions.Application");

    private static Counter<int> _subscriptionsStarted =
        _meter.CreateCounter<int>("subscriptions-subscriptions-started");
    
    private static Counter<int> _subscriptionsCancelled =
        _meter.CreateCounter<int>("subscriptions-subscriptions-cancelled");

    public static Counter<int> SubscriptionsStarted => _subscriptionsStarted;
    public static Counter<int> SubscriptionsCancelled => _subscriptionsCancelled;
}