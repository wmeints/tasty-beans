using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace TastyBeans.Simulation.Api.Tests.Controllers;

public static class TestServiceExtensions
{
    public static Mock<TInterface> ReplaceWithMock<TInterface>(this IServiceCollection services) where TInterface: class
    {
        var mock = new Mock<TInterface>();
        
        var serviceEntry = services.SingleOrDefault(
            x => x.ServiceType == typeof(TInterface));

        if (serviceEntry != null)
        {
            services.Remove(serviceEntry);
        }

        services.AddSingleton(mock.Object);

        return mock;
    }
}