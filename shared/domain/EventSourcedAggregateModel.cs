using System.Reflection;
using System.Runtime.CompilerServices;

namespace TastyBeans.Shared.Domain;

public class EventSourcedAggregateModel<T>
{
    public EventSourcedAggregateModel()
    {
        EventHandlers = TargetType
            .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Where(IsEventHandlerMethod)
            .ToDictionary(x => x.GetParameters().First().ParameterType);
    }
    
    public Type TargetType { get; } = typeof(T);
    
    public Dictionary<Type, MethodInfo> EventHandlers { get; }

    private bool IsEventHandlerMethod(MethodInfo methodInfo)
    {
        if (methodInfo.GetCustomAttribute<EventHandlerAttribute>() is not { })
        {
            return false;
        }

        var methodParameter = methodInfo.GetParameters().FirstOrDefault();

        if (methodParameter != null && !methodParameter.ParameterType.IsAssignableTo(typeof(IDomainEvent)))
        {
            return false;
        }

        return true;
    }
}