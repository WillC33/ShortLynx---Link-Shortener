using Microsoft.Azure.Cosmos;

namespace LinkShortener;

internal class CosmosRepository
{
    private readonly string _endpointUri;
    private readonly string _primaryKey;
    private const string DatabaseId = "ShortLynxDb";
    private const string ContainerId = "ShortenedLinks";
    
    private CosmosClient _cosmosClient;
    private Database _database;
    private Container _container;

    public CosmosRepository(IConfiguration configuration)
    {
        _endpointUri = configuration["COSMOSDB_ENDPOINT"];
        _primaryKey = configuration["COSMOSDB_KEY"];
        
        _cosmosClient = new CosmosClient(_endpointUri, _primaryKey);
        
        _database = _cosmosClient.GetDatabase(DatabaseId);
        _container = _database.GetContainer(ContainerId);
    }

    internal async Task WriteLink(ShortenedLinkModel link)
    {
        try
        {
            await _container.CreateItemAsync(link);
            Console.WriteLine("Link inserted successfully.");
        }
        catch (Exception ex)
        {
            throw new Exception($"Error inserting link: {ex.Message}");
        }
    }

    internal async Task<ShortenedLinkModel> ReadLink(string hash)
    {
        try
        {
            var response = await _container.ReadItemAsync<ShortenedLinkModel>(hash, new PartitionKey(hash));
            return response.Resource;
        }
        catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            // Document not found
            return null;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error querying link: {ex.Message}");
        }
    }
}