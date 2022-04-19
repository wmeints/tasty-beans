using RecommendCoffee.Timer.Domain.Common;

namespace RecommendCoffee.Timer.Application.Common;

public class EventValidationFailedException : Exception
{
    public EventValidationFailedException(IEnumerable<ValidationError> errors)
        : base("Event validation failed.")
    {
        Errors = errors;
    }

    public IEnumerable<ValidationError> Errors { get; }
}