using System.Diagnostics;

namespace TastyBeans.Shipping.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Shipping.Infrastructure");

    public static Activity? ExecuteDatabaseCommand()
    {
        return ActivitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}