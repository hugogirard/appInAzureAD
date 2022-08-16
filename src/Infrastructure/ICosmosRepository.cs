namespace PizzaShop.Infrastructure;

public interface ICosmosRepository
{
    Task<T> CreateReplaceItemAsync<T>(T item, string partitionKey) where T : class;
    Task<T> GetItemAsync<T>(string id, string partitionKey) where T : class;
    Task<IList<T>> QueryAsync<T>(string query) where T : class;
}
