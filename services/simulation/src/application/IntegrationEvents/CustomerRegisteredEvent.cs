using TastyBeans.Shared.Domain;

namespace TastyBeans.Simulation.Application.IntegrationEvents;

[Topic("customermanagement.customer.registered.v1")]
public record CustomerRegisteredEvent(Guid CustomerId);
