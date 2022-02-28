using Dapr.Client;
using RecommendCoffee.Registration.Application.Common;

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
        return await _daprClient.GetStateAsync<T>("state", key);
    }

    public async Task Put(string key, object data)
    {
        await _daprClient.SaveStateAsync("state", key, data);
    }
}