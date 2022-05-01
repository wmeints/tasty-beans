using Dapr.Client;
using TastyBeans.Shared.Domain;

namespace TastyBeans.Shared.Infrastructure.StateManagement;

public class StateStore: IStateStore
{
    private readonly DaprClient _daprClient;

    public StateStore(DaprClient daprClient)
    {
        _daprClient = daprClient;
    }

    public async Task<T> Get<T>(string key)
    {
        return await _daprClient.GetStateAsync<T>("statestore", key);
    }

    public async Task Set(string key, object data)
    {
        await _daprClient.SaveStateAsync("statestore", key, data);
    }
}