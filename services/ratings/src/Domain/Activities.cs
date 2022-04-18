using System.Diagnostics;

namespace RecommendCoffee.Ratings.Domain;

public class Activities
{
    private static ActivitySource _activitySource = new ActivitySource("RecommendCoffee.Ratings.Domain");

    public static Activity? RegisterRating(Guid customerId, Guid productId)
    {
        var activity = _activitySource.StartActivity("RegisterRating", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("customer.id", customerId.ToString());
            activity.AddTag("product.id", productId.ToString());
        }

        return activity;
    }

    public static Activity? RegisterProduct(Guid productId)
    {
        var activity = _activitySource.StartActivity("RegisterProduct", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("product.id", productId.ToString());
        }

        return activity;
    }
    
    public static Activity? RegisterCustomer(Guid customerId)
    {
        var activity = _activitySource.StartActivity("RegisterCustomer", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("customer.id", customerId.ToString());
        }

        return activity;
    }
}