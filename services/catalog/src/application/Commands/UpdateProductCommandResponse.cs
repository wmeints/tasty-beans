using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Commands;

public record UpdateProductCommandResponse(bool ProductExists, IEnumerable<BusinessRuleViolation> Errors)
{
    public bool IsValid => !Errors.Any();
}