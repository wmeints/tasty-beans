﻿using System.Diagnostics;

namespace TastyBeans.Catalog.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Catalog.Infrastructure");

    public static Activity? PublishEvent(string eventName)
    {
        var activity = ActivitySource.StartActivity("PublishEvent", ActivityKind.Client);

        if (activity != null)
        {
            activity.AddTag("event-name", eventName);
        }

        return activity;
    }

    public static Activity? ExecuteDatabaseCommand()
    {
        return ActivitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}