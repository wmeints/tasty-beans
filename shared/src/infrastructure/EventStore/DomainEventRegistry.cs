using System.Reflection;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Infrastructure.EventStore;

public static class DomainEventRegistry
{
    private static readonly Dictionary<string, Type> SchemaToType = new();
    private static readonly Dictionary<Type, string> TypeToSchema = new();

    public static void RegisterAssembly(Assembly assembly)
    {
        var domainEventTypes = assembly.GetTypes().Where(x => x.IsAssignableTo(typeof(IDomainEvent))).ToList();

        foreach (var domainEventType in domainEventTypes)
        {
            Register(domainEventType);
        }
    }
    
    public static void Register(Type type)
    {
        SchemaToType.Add(type.FullName!, type);
        TypeToSchema.Add(type, type.FullName!);
    }

    public static string? GetSchema(Type type)
    {
        if (TypeToSchema.ContainsKey(type))
        {
            return TypeToSchema[type];
        }

        return null;
    }

    public static Type? GetType(string schema)
    {
        if (SchemaToType.ContainsKey(schema))
        {
            return SchemaToType[schema];
        }

        return null;
    }
}