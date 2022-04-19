using Microsoft.Extensions.Logging;

namespace RecommendCoffee.Subscriptions.Infrastructure;

public static partial class Log
{
    [LoggerMessage(
        0, 
        LogLevel.Warning, 
        "Missing topic attribute on event payload. It will be published to the deadletter topic"
    )]
    public static partial void MissingTopicAttribute(this ILogger logger);
}