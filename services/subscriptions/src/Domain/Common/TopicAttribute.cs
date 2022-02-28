namespace RecommendCoffee.Subscriptions.Domain.Common;

[AttributeUsage(AttributeTargets.Class)]
public class TopicAttribute: Attribute
{
    public TopicAttribute(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
}