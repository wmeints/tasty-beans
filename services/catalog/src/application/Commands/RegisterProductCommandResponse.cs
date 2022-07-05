using System.Diagnostics.CodeAnalysis;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Application.Commands;

public record RegisterProductCommandResponse(Product? Product, IEnumerable<BusinessRuleViolation> Errors)
{
    [MemberNotNullWhen(true, nameof(Product))]
    public bool IsValid => !Errors.Any();
}