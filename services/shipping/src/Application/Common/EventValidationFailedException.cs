using RecommendCoffee.Shipping.Domain.Common;

namespace RecommendCoffee.Shipping.Application.Common;

public class EventValidationFailedException : Exception
{
    public EventValidationFailedException(IEnumerable<ValidationError> errors)
        : base("Event validation failed.")
    {
        Errors = errors;
    }

    public IEnumerable<ValidationError> Errors { get; }
}