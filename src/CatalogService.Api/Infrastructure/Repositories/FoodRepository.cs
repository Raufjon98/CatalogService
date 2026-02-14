using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class FoodRepository : IFoodRepository
{
    private readonly IMongoCollection<Food> _foods;

    public FoodRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database is null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _foods = mongoDbService.Database.GetCollection<Food>("Foods");
    }

    public async Task<Food> CreateAsync(Food food, CancellationToken cancellationToken = default)
    {
        await _foods.InsertOneAsync(food, cancellationToken: cancellationToken);
        return food;
    }

    public async Task<Food> UpdateAsync(Food food, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Food>.Filter.Eq(x => x.Id, food.Id);
        await _foods.ReplaceOneAsync(filter, food, cancellationToken: cancellationToken);
        return food;
    }

    public async Task<Food> UpdateAvailabilityAsync(string id, bool isAvailable, CancellationToken cancellationToken)
    {
        var filter = Builders<Food>.Filter.Eq(f => f.Id, id);
        var update = Builders<Food>.Update
            .Set(f => f.Availability, isAvailable)
            .Set(f => f.ModifiedAt, DateTime.UtcNow);

        var options = new FindOneAndUpdateOptions<Food>
        {
            ReturnDocument = ReturnDocument.After
        };
        return await _foods.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> DeleteAsync(Food food, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Food>.Filter.Eq(x => x.Id, food.Id);
        var update = Builders<Food>.Update
            .Set(f => f.IsDeleted, true)
            .Set(f => f.ModifiedAt, DateTime.UtcNow);
        var result = await _foods.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<List<Food>> GetAllAsync(string[]? ids = null, CancellationToken cancellationToken = default)
    {
        if (ids is {Length: > 0})
        {
            var filter = Builders<Food>.Filter.And(Builders<Food>.Filter.In(f=>f.Id, ids),
                Builders<Food>.Filter.Eq(f=>f.IsDeleted, false));
            
            return await _foods.Find(filter).ToListAsync(cancellationToken: cancellationToken);
        }
      
        return await _foods.Find(Builders<Food>.Filter.Gt(f => f.Stock, 0)).ToListAsync(cancellationToken);
    }

    public async Task<Food?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Food>.Filter.And(Builders<Food>.Filter.Eq(x => x.Id, id),
            Builders<Food>.Filter.Eq(f => f.IsDeleted, false));
        
        var food = await _foods.Find(filter).FirstOrDefaultAsync(cancellationToken);
        return food;
    }

    public async Task<List<Food>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken)
    {
        var filter = Builders<Food>.Filter.And(Builders<Food>.Filter.Eq(f => f.FoodCategoryId, categoryId),
            Builders<Food>.Filter.Eq(f => f.Availability, true), 
            Builders<Food>.Filter.Eq(f => f.IsDeleted, false));
        return await _foods.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Food>> GetByRestaurantIdAsync(string restaurantId, CancellationToken cancellationToken)
    {
        var filter = Builders<Food>.Filter.And(Builders<Food>.Filter.Eq(f => f.RestaurantId, restaurantId),
            Builders<Food>.Filter.Eq(f => f.Availability, true), 
            Builders<Food>.Filter.Eq(f => f.IsDeleted, false));
        return await _foods.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Food>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice,
        CancellationToken cancellationToken)
    {
        var filterDefinition = Builders<Food>.Filter;
        var filter = filterDefinition.Eq(f => f.Availability, true) &
                     filterDefinition.Gte(f => f.Price, minPrice) &
                     filterDefinition.Lte(f => f.Price, maxPrice) &
                     filterDefinition.Eq(f => f.IsDeleted, false);
        
        return await _foods.Find(filter).ToListAsync(cancellationToken);
    }
}