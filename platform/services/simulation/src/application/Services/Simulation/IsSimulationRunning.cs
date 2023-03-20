namespace TastyBeans.Simulation.Application.Services.Simulation;

public record IsSimulationRunning
{
    public static IsSimulationRunning Instance { get; } = new IsSimulationRunning();
}