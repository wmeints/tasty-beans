using System.Diagnostics;

namespace RecommendCoffee.Registration.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Registration.Infrastructure");

    public static Activity? PublishEvent(string eventName)
    {
        var activity = ActivitySource.StartActivity("PublishEvent", ActivityKind.Client);

        if (activity != null)
        {
            activity.AddTag("event-name", eventName);
        }

        return activity;
    }

    public static Activity? SetState(string key)
    {
        var activity = ActivitySource.StartActivity("SetState", ActivityKind.Client);
        activity?.AddTag("state.key", key);

        return activity;
    }
    
    public static Activity? GetState(string key)
    {
        var activity = ActivitySource.StartActivity("GetState", ActivityKind.Client);
        activity?.AddTag("state.key", key);

        return activity;
    }
}