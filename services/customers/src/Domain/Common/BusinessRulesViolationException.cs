namespace RecommendCoffee.Customers.Domain.Common;

public class BusinessRulesViolationException : Exception
{
    public BusinessRulesViolationException(IEnumerable<BusinessRuleViolation> errors)
        : base("One ore more business rule violations occurred while processing of your request.")
    {
        Errors = errors;
    }

    public IEnumerable<BusinessRuleViolation> Errors { get; }
}
