using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Application.Common;

public class EventValidationFailedException : Exception
{
    public EventValidationFailedException(IEnumerable<ValidationError> errors)
        : base("Event validation failed.")
    {
        Errors = errors;
    }

    public IEnumerable<ValidationError> Errors { get; }
}