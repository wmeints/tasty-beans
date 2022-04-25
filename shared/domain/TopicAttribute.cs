namespace RecommendCoffee.Shared.Domain;

[AttributeUsage(AttributeTargets.Class)]
public class TopicAttribute: Attribute
{
    public TopicAttribute(string name)
    {
        Name = name;
    }
    
    public string Name { get; }
}