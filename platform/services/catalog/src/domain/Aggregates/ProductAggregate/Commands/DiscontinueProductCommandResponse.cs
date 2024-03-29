﻿using TastyBeans.Shared.Domain;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

public record DiscontinueProductCommandResponse(IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    public bool IsValid => !Errors.Any();
}