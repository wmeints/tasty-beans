using System.Diagnostics;

namespace TastyBeans.Recommendations.Application;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Recommendations.Application");

    public static Activity? HandleEvent(string eventName)
    {
        var activity = ActivitySource.StartActivity("HandleEvent", ActivityKind.Internal);

        activity?.AddTag("event.name", eventName);

        return activity;
    }
}