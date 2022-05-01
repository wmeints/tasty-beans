using System.Diagnostics.Metrics;

namespace TastyBeans.Payments.Application;

public class Metrics
{
    private static Meter _meter = new Meter("TastyBeans.Payments.Application");

    public static Counter<int> PaymentMethodRegistered { get; } = _meter.CreateCounter<int>("payments-paymentmethods-registered");
}