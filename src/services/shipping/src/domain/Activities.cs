using System.Diagnostics;

namespace TastyBeans.Shipping.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Shipping.Domain");

    public static Activity? RegisterProduct(Guid productId)
    {
        var activity = ActivitySource.StartActivity("RegisterProduct", ActivityKind.Internal);
        
        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }
}