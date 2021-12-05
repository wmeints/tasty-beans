namespace RecommendCoffee.Customers.Domain.Common;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class TopicAttribute : Attribute
{
    public TopicAttribute(string topicName)
    {
        TopicName = topicName;
    }

    public string TopicName { get; set; }
}
