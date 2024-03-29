﻿using System.Diagnostics;

namespace TastyBeans.Subscriptions.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Subscriptions.Domain");

    public static Activity? Subscribe(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("Register", ActivityKind.Internal);
        activity?.AddTag("customer.id", customerId.ToString());

        return activity;
    }

    public static Activity? ChangeShippingFrequency(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("ChangeShippingFrequency", ActivityKind.Internal);
        activity?.AddTag("customer.id", customerId.ToString());

        return activity;
    }

    public static Activity? Unsubscribe(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("Unsubscribe", ActivityKind.Internal);
        activity?.AddTag("customer.id", customerId.ToString());

        return activity;
    }
}