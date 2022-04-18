using System.Diagnostics;

namespace RecommendCoffee.Catalog.Domain;

public class Activities
{
    private static ActivitySource _activitySource = new ActivitySource("RecommendCoffee.Catalog.Domain");

    public static Activity? TasteTest(Guid productId)
    {
        var activity = _activitySource.StartActivity("TasteTest", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("product.id", productId.ToString());
        }

        return activity;
    }

    public static Activity? Register()
    {
        return _activitySource.StartActivity("Register", ActivityKind.Internal);
    }

    public static Activity? Discontinue(Guid productId)
    {
        var activity = _activitySource.StartActivity("Discontinue", ActivityKind.Internal);
        
        if (activity != null)
        {
            activity.AddTag("product.id", productId.ToString());    
        }
        
        return activity;
    }

    public static Activity? Update(Guid productId)
    {
        var activity = _activitySource.StartActivity("Update", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("product.id", productId);
        }

        return activity;
    }
}