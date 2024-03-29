﻿using TastyBeans.Ratings.Application.IntegrationEvents;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate;
using TastyBeans.Ratings.Domain.Aggregates.CustomerAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.Ratings.Application.EventHandlers;

public class CustomerRegisteredEventHandler
{
    private readonly ICustomerRepository _customerRepository;

    public CustomerRegisteredEventHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public async Task HandleAsync(CustomerRegisteredEvent evt)
    {
        using var activity = Activities.HandleEvent("customermanagement.customer.registered.v1");
        var response = Customer.Register(new RegisterCustomerCommand(
            evt.CustomerId, evt.FirstName, evt.LastName));

        if (!response.IsValid)
        {
            throw new EventValidationFailedException(response.Errors);
        }

        await _customerRepository.InsertAsync(response.Customer);
    }
}