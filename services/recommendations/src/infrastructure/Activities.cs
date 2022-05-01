using System.Diagnostics;

namespace TastyBeans.Recommendations.Infrastructure;

public static class Activities
{
    private static readonly ActivitySource ActivitySource = new ActivitySource("TastyBeans.Recommendations.Infrastructure");
    
    public static Activity? ExecuteDatabaseCommand()
    {
        return ActivitySource.StartActivity("ExecuteDbCommand", ActivityKind.Client);
    }
}