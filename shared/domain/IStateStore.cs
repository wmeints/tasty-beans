namespace RecommendCoffee.Shared.Domain;

public interface IStateStore
{
    Task<T> Get<T>(string key);
    Task Set(string key, object data);
}