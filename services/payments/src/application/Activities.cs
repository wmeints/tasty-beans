using System.Diagnostics;

namespace RecommendCoffee.Payments.Application;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Payments.Application");

    public static Activity? ExecuteCommand(string commandName)
    {
        var activity = ActivitySource.StartActivity("ExecuteCommand", ActivityKind.Internal);

        activity?.AddTag("command.name", commandName);

        return activity;
    }
    
    public static Activity? HandleEvent(string eventName)
    {
        var activity = ActivitySource.StartActivity("HandleEvent", ActivityKind.Internal);

        activity?.AddTag("event.name", eventName);

        return activity;
    }

    public static Activity? ExecuteQuery(string queryName)
    {
        var activity = ActivitySource.StartActivity("ExecuteQuery", ActivityKind.Internal);

        activity?.AddTag("query.name", queryName);

        return activity;
    }
}