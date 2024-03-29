﻿using FluentValidation;
using TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Commands;

namespace TastyBeans.Catalog.Domain.Aggregates.ProductAggregate.Validators;

public class TasteTestCommandValidator: AbstractValidator<TasteTestProductCommand>
{
    public TasteTestCommandValidator()
    {
        RuleFor(x => x.Taste).NotEmpty().MaximumLength(100);
        RuleFor(x => x.RoastLevel).LessThanOrEqualTo(10).GreaterThanOrEqualTo(1);
        RuleFor(x => x.FlavorNotes).NotEmpty();
    }
}