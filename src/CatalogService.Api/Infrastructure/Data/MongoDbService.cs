using MongoDB.Driver;

namespace CatalogService.Api.Features.Data;

public class MongoDbService
{
    private readonly IMongoDatabase _database;
    public MongoDbService(IMongoClient client, IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("MongoDb")
            ?? throw new InvalidOperationException("MongoDB connection string 'MongoDb' is not configured.");
        
        var mongoUrl = new MongoUrl(connectionString);
        
        if (string.IsNullOrWhiteSpace(mongoUrl.DatabaseName))
            throw new InvalidOperationException(
                "MongoDB database name must be specified in the connection string.");
        
        _database = client.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database; 
}