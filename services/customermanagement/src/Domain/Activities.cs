using System.Diagnostics;

namespace RecommendCoffee.CustomerManagement.Domain;

public class Activities
{
    private static readonly ActivitySource ActivitySource = 
        new ActivitySource("RecommendCoffee.Ratings.Infrastructure");

    public static Activity? Register(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("Register", ActivityKind.Internal);
        activity?.AddTag("customer.id", customerId);

        return activity;
    }
}