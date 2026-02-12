using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly IMongoCollection<Restaurant> _restaurants;

    public RestaurantRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database is null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _restaurants = mongoDbService.Database.GetCollection<Restaurant>("Restaurants");
    }

    public async Task<Restaurant> CreateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        await _restaurants.InsertOneAsync(restaurant, cancellationToken: cancellationToken);
        return restaurant;
    }

    public async Task<Restaurant> UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.Eq(x => x.Id, restaurant.Id);
        await _restaurants.ReplaceOneAsync(filter, restaurant, cancellationToken: cancellationToken);
        return restaurant;
    }

    public async Task<bool> DeleteAsync(Restaurant restaurant, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.Eq(x => x.Id, restaurant.Id);
        var update = Builders<Restaurant>.Update
            .Set(r=>r.IsDeleted, true)
            .Set(r => r.ModifiedAt, DateTime.UtcNow);
        var result = await _restaurants.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<List<Restaurant>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _restaurants.Find(Builders<Restaurant>.Filter.Eq(r=>r.IsDeleted, false)).ToListAsync(cancellationToken);
    }

    public async Task<Restaurant?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.And(Builders<Restaurant>.Filter.Eq(r => r.Id, id),
            Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<List<Restaurant>> GetPagedAsync(int page, int pageSize,
        CancellationToken cancellationToken = default)
    {
        return await _restaurants.Find(Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false))
            .Skip((page - 1) * pageSize).Limit(pageSize).ToListAsync(cancellationToken);
    }

    public async Task<Restaurant> UpdateAvailabilityAsync(string id, bool isAvailable, CancellationToken cancellationToken)
    {
        var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, id);
        var update = Builders<Restaurant>.Update
            .Set(r => r.Availability, isAvailable)
            .Set(r => r.ModifiedAt, DateTime.UtcNow);
         
        var options = new FindOneAndUpdateOptions<Restaurant>
        {
            ReturnDocument = ReturnDocument.After
        };

        return await _restaurants.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> UpdateCategoryAsync(string id, string categoryId,
        CancellationToken cancellationToken)
    {
        var filter = Builders<Restaurant>.Filter.Eq(r => r.Id, id);
        var update = Builders<Restaurant>.Update
            .Set(r => r.CategoryId, categoryId)
            .Set(r => r.ModifiedAt, DateTime.UtcNow);

        var result = await _restaurants.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteInactiveAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.Eq(r => r.IsActive, false);
        var update = Builders<Restaurant>.Update
            .Set(r => r.IsDeleted, true)
            .Set(r => r.ModifiedAt, DateTime.UtcNow);
        
        var result = await _restaurants.UpdateManyAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }

    public async Task<List<Restaurant>> GetByCategoryAsync(string categoryId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.And(Builders<Restaurant>.Filter.Eq(r => r.CategoryId, categoryId),
                Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Restaurant>> GetAvailableRestaurantsAsync(CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.And(Builders<Restaurant>.Filter.Eq(r => r.Availability, true),
            Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Restaurant>> GetByCuisineAsync(string cuisineId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<Restaurant>.Filter.And(Builders<Restaurant>.Filter.Eq(r => r.CuisineId, cuisineId),
            Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Restaurant>> GetActiveRestaurantsAsync(CancellationToken cancellationToken = default)
    {
        var filter =Builders<Restaurant>.Filter.And( Builders<Restaurant>.Filter.Eq(r => r.IsActive, true),
                Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).ToListAsync(cancellationToken);
    }

    public async Task<List<Restaurant>> SearchByNameAsync(string searchTerm,
        CancellationToken cancellationToken = default)
    {
        var filter =Builders<Restaurant>.Filter.And( Builders<Restaurant>.Filter.Regex(r => r.Name, new BsonRegularExpression(searchTerm, "i")),
                Builders<Restaurant>.Filter.Eq(r => r.IsDeleted, false));
        return await _restaurants.Find(filter).ToListAsync(cancellationToken);
    }
}