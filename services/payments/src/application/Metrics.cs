using System.Diagnostics.Metrics;

namespace RecommendCoffee.Payments.Application;

public class Metrics
{
    private static Meter _meter = new Meter("RecommendCoffee.Payments.Application");
    private static Counter<int> _paymentMethodRegistered = _meter.CreateCounter<int>("payments-paymentmethods-registered");

    public static Counter<int> PaymentMethodRegistered => _paymentMethodRegistered;
}