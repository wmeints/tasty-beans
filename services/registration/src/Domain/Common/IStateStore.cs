namespace RecommendCoffee.Registration.Domain.Common;

public interface IStateStore
{
    Task<T> Get<T>(string key);
    Task Put(string key, object data);
}