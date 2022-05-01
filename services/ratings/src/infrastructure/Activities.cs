using System.Diagnostics;

namespace TastyBeans.Ratings.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Ratings.Infrastructure");

    public static Activity? ExecuteDatabaseCommand()
    {
        return ActivitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}