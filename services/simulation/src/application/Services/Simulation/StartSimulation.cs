using Akka.Actor;
using TastyBeans.Simulation.Domain.Aggregates.CustomerAggregate;

namespace TastyBeans.Simulation.Application.Services.Simulation;

public record StartSimulation(int CustomerCount, List<WeightedCustomerProfile> CustomerProfiles);