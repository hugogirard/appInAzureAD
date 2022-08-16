using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace PizzaShop.Infrastructure;

public class CosmosRepository : ICosmosRepository
{
    private readonly Container _container;

    public CosmosRepository(string cnxString, string databaseName, string containerName)
    {
        var cosmosClient = new CosmosClient(cnxString);
        var database = cosmosClient.GetDatabase(databaseName);
        _container = database.GetContainer(containerName);
    }

    public async Task<T> GetItemAsync<T>(string id, string partitionKey) where T : class
    {
        return await _container.ReadItemAsync<T>(id, new PartitionKey(partitionKey));
    }

    public async Task<T> CreateReplaceItemAsync<T>(T item, string partitionKey) where T : class
    {
        await _container.UpsertItemAsync(item, new PartitionKey(partitionKey));
        return item;
    }

    public async Task<IList<T>> QueryAsync<T>(string query) where T : class
    {
        var queryDefinition = new QueryDefinition(query);
        var documents = new List<T>();

        using FeedIterator<T> feed = _container.GetItemQueryIterator<T>(queryDefinition);

        while (feed.HasMoreResults)
        {
            var items = await feed.ReadNextAsync();
            foreach (var item in items)
            {
                documents.Add(item);
            }
        }

        return documents;
    }
}
