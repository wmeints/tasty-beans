using Akka.Actor;
using Akka.Event;
using TastyBeans.Simulation.Application.Services.Registration;

namespace TastyBeans.Simulation.Application.Services.Simulation;

public class CustomerRegistrationWorker: ReceiveActor
{
    private readonly IRegistration _registration;

    public CustomerRegistrationWorker(IRegistration registration)
    {
        _registration = registration;
        Receive<RegistrationRequest>(msg => _registration.RegisterCustomerAsync(msg));
    }

    protected override void PreRestart(Exception reason, object message)
    {
        Context.GetLogger().Log(LogLevel.ErrorLevel, reason, "Failed to register customer");
    }

    public static Props Props(IRegistration registration)
    {
        return new Props(
            type: typeof(CustomerRegistrationWorker), 
            supervisorStrategy: new OneForOneStrategy(ex => Directive.Stop),
            args: registration);
    }
}