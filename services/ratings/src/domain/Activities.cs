using System.Diagnostics;

namespace RecommendCoffee.Ratings.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Ratings.Domain");

    public static Activity? RegisterRating(Guid customerId, Guid productId)
    {
        var activity = ActivitySource.StartActivity("RegisterRating", ActivityKind.Internal);

        activity?.AddTag("customer.id", customerId.ToString());
        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }

    public static Activity? RegisterProduct(Guid productId)
    {
        var activity = ActivitySource.StartActivity("RegisterProduct", ActivityKind.Internal);
        
        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }
    
    public static Activity? RegisterCustomer(Guid customerId)
    {
        var activity = ActivitySource.StartActivity("RegisterCustomer", ActivityKind.Internal);
        
        activity?.AddTag("customer.id", customerId.ToString());

        return activity;
    }
}