using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class CuisineRepository : ICuisineRepository
{
    private readonly IMongoCollection<Cuisine> _cuisines;

    public CuisineRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database == null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _cuisines = mongoDbService.Database.GetCollection<Cuisine>("Cuisines");
    }

    public async Task<List<Cuisine>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _cuisines.Find(Builders<Cuisine>.Filter.Eq(c=>c.IsDeleted, false)).ToListAsync(cancellationToken);
    }

    public async Task<Cuisine?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Cuisine>.Filter.And(Builders<Cuisine>.Filter.Eq(x => x.Id, id),
                Builders<Cuisine>.Filter.Eq(x => x.IsDeleted, false));
        return await _cuisines.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Cuisine> CreateAsync(Cuisine cuisine, CancellationToken cancellationToken = default)
    {
        await _cuisines.InsertOneAsync(cuisine, cancellationToken: cancellationToken);
        return cuisine;
    }

    public async Task<Cuisine?> UpdateAsync(Cuisine cuisine, CancellationToken cancellationToken)
    {
        var filter = Builders<Cuisine>.Filter.Eq(x => x.Id, cuisine.Id);
        var update = Builders<Cuisine>.Update
            .Set(x => x.Name, cuisine.Name)
            .Set(x => x.Description, cuisine.Description)
            .Set(x => x.ModifiedAt, DateTime.Now);
        var options = new FindOneAndUpdateOptions<Cuisine>()
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false
        };
        return await _cuisines.FindOneAndUpdateAsync(filter, update, options);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var filter = Builders<Cuisine>.Filter.Eq(x => x.Id, id);
        var update = Builders<Cuisine>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.ModifiedAt, DateTime.UtcNow);
        var result = await _cuisines.UpdateOneAsync(filter,update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}