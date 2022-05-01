using System.Diagnostics;

namespace TastyBeans.Shipping.Application;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Shipping.Application");

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
}