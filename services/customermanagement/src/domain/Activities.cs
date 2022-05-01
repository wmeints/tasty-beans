using System.Diagnostics;

namespace TastyBeans.CustomerManagement.Domain;

public class Activities
{
    private static readonly ActivitySource ActivitySource = 
        new ActivitySource("TastyBeans.Ratings.Infrastructure");

    public static Activity? Register(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("Register", ActivityKind.Internal);
        activity?.AddTag("customer.id", customerId);

        return activity;
    }
}