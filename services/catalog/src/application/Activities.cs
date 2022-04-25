using System.Diagnostics;

namespace RecommendCoffee.Catalog.Application;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("RecommendCoffee.Catalog.Infrastructure");

    public static Activity? ExecuteCommand(string commandName)
    {
        var activity = ActivitySource.StartActivity("ExecuteCommand", ActivityKind.Internal);

        activity?.AddTag("command.name", commandName);

        return activity;
    }

    public static Activity? ExecuteQuery(string queryName)
    {
        var activity = ActivitySource.StartActivity("ExecuteQuery", ActivityKind.Internal);

        activity?.AddTag("query.name", queryName);

        return activity;
    }
}