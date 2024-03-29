﻿using System.Diagnostics.CodeAnalysis;
using TastyBeans.Shared.Domain;

namespace TastyBeans.CustomerManagement.Domain.Aggregates.CustomerAggregate.Commands;

public record RegisterCustomerCommandReply(Customer? Customer, IEnumerable<ValidationError> Errors, IEnumerable<IDomainEvent> Events)
{
    [MemberNotNullWhen(true, nameof(Customer))]
    public bool IsValid => !Errors.Any();
}