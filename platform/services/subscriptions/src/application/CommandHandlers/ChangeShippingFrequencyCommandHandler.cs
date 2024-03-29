﻿using TastyBeans.Shared.Application;
using TastyBeans.Shared.Domain;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate;
using TastyBeans.Subscriptions.Domain.Aggregates.SubscriptionAggregate.Commands;

namespace TastyBeans.Subscriptions.Application.CommandHandlers;

public class ChangeShippingFrequencyCommandHandler
{
    private readonly ISubscriptionRepository _subscriptionRepository;
    private readonly IEventPublisher _eventPublisher;

    public ChangeShippingFrequencyCommandHandler(ISubscriptionRepository subscriptionRepository, IEventPublisher eventPublisher)
    {
        _subscriptionRepository = subscriptionRepository;
        _eventPublisher = eventPublisher;
    }

    public async Task<ChangeShippingFrequencyCommandReply> ExecuteAsync(ChangeShippingFrequencyCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("ChangeShippingFrequency");
        
        var subscription = await _subscriptionRepository.FindByCustomerIdAsync(cmd.CustomerId);

        if (subscription == null)
        {
            throw new AggregateNotFoundException($"Can't find subscription for customer {cmd.CustomerId}");
        }

        var response = subscription.ChangeShippingFrequency(cmd);

        if (response.IsValid)
        {
            await _subscriptionRepository.UpdateAsync(subscription);
            await _eventPublisher.PublishEventsAsync(response.Events);
        }

        return response;
    }
}