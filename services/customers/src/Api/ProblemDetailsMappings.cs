using Microsoft.AspNetCore.Mvc;
using RecommendCoffee.Customers.Application.Persistence;
using RecommendCoffee.Customers.Domain.Common;

namespace RecommendCoffee.Customers.Api;

public static class ProblemDetailsMappings
{
    public static ProblemDetails BusinessRulesViolation(BusinessRulesViolationException ex)
    {
        var details = new ValidationProblemDetails
        {
            Type = "https://recommend.coffee/problems/business-rules-violation",
            Title = ex.Message
        };

        var violations = ex.Errors.GroupBy(
            x => x.Property,
            (key, values) => (key, values.Select(x => x.Message).ToArray())
        );

        foreach (var (propertyName, errorMessages) in violations)
        {
            details.Errors.Add(propertyName, errorMessages);
        }

        return details;
    }

    public static ProblemDetails AggregateNotFound(AggregateNotFoundException ex)
    {
        return new ProblemDetails
        {
            Status = 404,
            Type = "https://httpstatuses.com/404",
            Title = ex.Message
        };
    }
}
