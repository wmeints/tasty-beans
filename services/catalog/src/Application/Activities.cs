using System.Diagnostics;

namespace RecommendCoffee.Catalog.Application;

public class Activities
{
    private static ActivitySource _activitySource = new ActivitySource("RecommendCoffee.Catalog.Infrastructure");

    public static Activity? ExecuteCommand(string commandName)
    {
        var activity = _activitySource.StartActivity("ExecuteCommand", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("command.name", commandName);
        }

        return activity;
    }

    public static Activity? ExecuteQuery(string queryName)
    {
        var activity = _activitySource.StartActivity("ExecuteQuery", ActivityKind.Internal);

        if (activity != null)
        {
            activity.AddTag("query.name", queryName);
        }

        return activity;
    }
}