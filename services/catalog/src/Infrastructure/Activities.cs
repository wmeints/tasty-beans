using System.Diagnostics;

namespace RecommendCoffee.Catalog.Infrastructure;

public class Activities
{
    private static ActivitySource _activitySource = new ActivitySource("RecommendCoffee.Catalog.Infrastructure");

    public static Activity? PublishEvent(string eventName)
    {
        var activity = _activitySource.StartActivity("PublishEvent", ActivityKind.Client);

        if (activity != null)
        {
            activity.AddTag("event-name", eventName);
        }

        return activity;
    }

    public static Activity? ExecuteDatabaseCommand()
    {
        return _activitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}