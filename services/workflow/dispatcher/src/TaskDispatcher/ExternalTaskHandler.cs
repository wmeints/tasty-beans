using Camunda.Api.Client;

namespace RecommendCoffee.TaskDispatcher;

public abstract class ExternalTaskHandler
{
    public abstract string TopicName { get; }
    public abstract Task<Dictionary<string, VariableValue>> ExecuteAsync(
        Dictionary<string, VariableValue> inputVariables);
}