using System.Diagnostics;

namespace TastyBeans.Recommendations.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Recommendations.Domain");

    public static Activity? RegisterProduct(Guid productId)
    {
        var activity = ActivitySource.StartActivity("RegisterProduct", ActivityKind.Internal);
        
        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }
}