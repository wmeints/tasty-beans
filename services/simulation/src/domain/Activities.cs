using System.Diagnostics;

namespace TastyBeans.Simulation.Domain;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Catalog.Domain");

    public static Activity? TasteTest(Guid productId)
    {
        var activity = ActivitySource.StartActivity("TasteTest", ActivityKind.Internal);

        activity?.AddTag("product.id", productId.ToString());

        return activity;
    }

    public static Activity? ShipmentLost()
    {
        return ActivitySource.StartActivity("ShipmentLost");
    }

    public static Activity? ShipmentDelivered()
    {
        return ActivitySource.StartActivity("ShipmentDelivered");
    }
}