﻿using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate.Commands;
using RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate.Validators;
using RecommendCoffee.Ratings.Domain.Aggregates.ProductAggregate.Commands;
using RecommendCoffee.Ratings.Domain.Common;

namespace RecommendCoffee.Ratings.Domain.Aggregates.CustomerAggregate;

public class Customer
{
    public Customer(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public static RegisterCustomerCommandResponse Register(RegisterCustomerCommand cmd)
    {
        var validator = new RegisterCustomerCommandValidator();
        var validationResult = validator.Validate(cmd);

        if (!validationResult.IsValid)
        {
            return new RegisterCustomerCommandResponse(
                null, validationResult.GetValidationErrors());
        }

        var customer = new Customer(cmd.Id, cmd.FirstName, cmd.LastName);

        return new RegisterCustomerCommandResponse(customer, Enumerable.Empty<ValidationError>());
    }
}