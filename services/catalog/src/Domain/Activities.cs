using System.Diagnostics;

namespace RecommendCoffee.Catalog.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Catalog.Domain");

    public static Activity? TasteTest(Guid productId)
    {
        var activity = ActivitySource.StartActivity("TasteTest", ActivityKind.Internal);

        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }

    public static Activity? Register()
    {
        return ActivitySource.StartActivity("Register", ActivityKind.Internal);
    }

    public static Activity? Discontinue(Guid productId)
    {
        var activity = ActivitySource.StartActivity("Discontinue", ActivityKind.Internal);

        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }

    public static Activity? Update(Guid productId)
    {
        var activity = ActivitySource.StartActivity("Update", ActivityKind.Internal);

        activity?.AddTag("product.id", productId);

        return activity;
    }
}