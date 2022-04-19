using Dapr.Client;
using RecommendCoffee.Registration.Domain.Common;

namespace RecommendCoffee.Registration.Infrastructure.StateManagement;

public class StateStore: IStateStore
{
    private readonly DaprClient _daprClient;

    public StateStore(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<T> Get<T>(string key)
    {
        using var activity = Activities.GetState(key);
        return await _daprClient.GetStateAsync<T>("statestore", key);
    }

    public async Task Set(string key, object data)
    {
        using var activity = Activities.SetState(key);
        await _daprClient.SaveStateAsync("statestore", key, data);
    }
}