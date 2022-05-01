using System.Diagnostics;

namespace TastyBeans.Subscriptions.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Subscriptions.Infrastructure");

    public static Activity? ExecuteDatabaseCommand()
    {
        return ActivitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}