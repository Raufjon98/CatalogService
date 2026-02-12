using MongoDB.Driver;

namespace CatalogService.Api.Features.Data;

public class MongoDbService
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;
    public MongoDbService(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString =
            _configuration.GetConnectionString("MongoDb") 
            ?? throw new InvalidOperationException("MongoDB connection string 'MongoDb' is not configured.");;
        var mongoUrl = new MongoUrl(connectionString);
        if (string.IsNullOrWhiteSpace(mongoUrl.DatabaseName))
            throw new InvalidOperationException(
                "MongoDB database name must be specified in the connection string.");
        var client = new MongoClient(mongoUrl);
        _database = client.GetDatabase(mongoUrl.DatabaseName);
    }

    public IMongoDatabase? Database => _database; 
}