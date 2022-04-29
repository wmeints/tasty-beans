using System.Diagnostics;
using Castle.DynamicProxy;

namespace RecommendCoffee.Shared.Diagnostics;

[Serializable]
public class TracingInterceptor: IInterceptor
{
    private readonly ActivitySourceRegistry _activitySourceRegistry;

    public TracingInterceptor(ActivitySourceRegistry activitySourceRegistry)
    {
        _activitySourceRegistry = activitySourceRegistry;
    }

    public void Intercept(IInvocation invocation)
    {
        var sourceName = invocation.TargetType.Assembly.GetName().Name;
        var activitySource = _activitySourceRegistry.Get(sourceName);

        using (var activity = activitySource.StartActivity(invocation.Method.Name))
        {
            invocation.Proceed();
        }
    }
}