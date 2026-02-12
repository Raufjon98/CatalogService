using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class FoodCategoryRepository : IFoodCategoryRepository
{
    private readonly IMongoCollection<FoodCategory> _foodCategories;

    public FoodCategoryRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database == null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _foodCategories = mongoDbService.Database.GetCollection<FoodCategory>("FoodCategories");
    }
    public async Task<List<FoodCategory>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _foodCategories.Find(Builders<FoodCategory>.Filter.Eq(f=>f.IsDeleted, false)).ToListAsync(cancellationToken);
    }

    public async Task<FoodCategory?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<FoodCategory>.Filter.And(Builders<FoodCategory>.Filter.Eq(x => x.Id, id),
                Builders<FoodCategory>.Filter.Eq(x => x.IsDeleted, false));
        
        return await _foodCategories.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<FoodCategory> CreateAsync(FoodCategory foodCategory, CancellationToken cancellationToken = default)
    {
        await _foodCategories.InsertOneAsync(foodCategory, cancellationToken: cancellationToken);
        return foodCategory;
    }

    public async Task<FoodCategory?> UpdateAsync(FoodCategory foodCategory, CancellationToken cancellationToken = default)
    {
        var filter = Builders<FoodCategory>.Filter.Eq(x => x.Id, foodCategory.Id);
        var update = Builders<FoodCategory>.Update
            .Set(x => x.Name, foodCategory.Name)
            .Set(x => x.Availability, foodCategory.Availability)
            .Set(x => x.ModifiedAt, DateTime.Now);

        var options = new FindOneAndUpdateOptions<FoodCategory>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false
        };
        return await _foodCategories.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<FoodCategory>.Filter.Eq(x => x.Id, id);
        var update = Builders<FoodCategory>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.ModifiedAt, DateTime.UtcNow);
        
        var result = await _foodCategories.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}