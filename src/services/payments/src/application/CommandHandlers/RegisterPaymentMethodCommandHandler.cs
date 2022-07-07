using Microsoft.Extensions.Logging;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate;
using RecommendCoffee.Payments.Domain.Aggregates.PaymentMethodAggregate.Commands;
using TastyBeans.Shared.Application;

namespace TastyBeans.Payments.Application.CommandHandlers;

public class RegisterPaymentMethodCommandHandler
{
    private readonly IPaymentMethodRepository _paymentMethodRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly ILogger<RegisterPaymentMethodCommandHandler> _logger;

    public RegisterPaymentMethodCommandHandler(
        IPaymentMethodRepository paymentMethodRepository, 
        ILogger<RegisterPaymentMethodCommandHandler> logger, 
        IEventPublisher eventPublisher)
    {
        _paymentMethodRepository = paymentMethodRepository;
        _logger = logger;
        _eventPublisher = eventPublisher;
    }

    public async Task<RegisterPaymentMethodReply> ExecuteAsync(RegisterPaymentMethodCommand cmd)
    {
        using var activity = Activities.ExecuteCommand("RegisterPaymentMethod");
        
        var response = PaymentMethod.Register(cmd);

        if (response.IsValid)
        {
            await _paymentMethodRepository.InsertAsync(response.PaymentMethod);
            await _eventPublisher.PublishEventsAsync(response.Events);
            
            Metrics.PaymentMethodRegistered.Add(1);
        }

        return response;
    }
}