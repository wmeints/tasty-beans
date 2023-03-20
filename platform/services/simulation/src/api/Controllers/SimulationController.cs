using Microsoft.AspNetCore.Mvc;
using TastyBeans.Simulation.Api.Forms;
using TastyBeans.Simulation.Application.Services;

namespace TastyBeans.Simulation.Api.Controllers;

[ApiController]
[Route("/")]
public class SimulationController: ControllerBase
{
    private readonly ISimulation _simulation;

    public SimulationController(ISimulation simulation)
    {
        _simulation = simulation;
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartSimulation(StartSimulationForm form)
    {
        await _simulation.StartSimulationAsync(form.CustomerCount, form.Profiles);
        return Accepted();
    }
}