namespace RecommendCoffee.Ratings.Api;

public static partial class ApplicationLog
{
    [LoggerMessage(1001, LogLevel.Information, "Handling catalog.product.registered.v1")]
    public static partial void HandlingProductRegisteredEvent(this ILogger logger);

    [LoggerMessage(1002, LogLevel.Information, "Handling catalog.product.updated.v1")]
    public static partial void HandlingProductUpdatedEvent(this ILogger logger);
    
    [LoggerMessage(1003, LogLevel.Information, "Handling catalog.product.discontinued.v1")]
    public static partial void HandlingProductDiscontinuedEvent(this ILogger logger);
    
    [LoggerMessage(1004, LogLevel.Information, "Handling customermanagement.customer.registered.v1")]
    public static partial void HandlingCustomerRegisteredEvent(this ILogger logger);
    
}