using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly IMongoCollection<Category> _categories;

    public CategoryRepository(MongoDbService  mongoDbService)
    {
        if (mongoDbService.Database is null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _categories = mongoDbService.Database.GetCollection<Category>("Categories");
    }
    public async Task<List<Category>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _categories.Find(Builders<Category>.Filter.Eq(c=>c.IsDeleted, false)).ToListAsync(cancellationToken);
    }

    public async Task<Category?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.And(Builders<Category>.Filter.Eq(x => x.Id, id),
                Builders<Category>.Filter.Eq(x => x.IsDeleted, false));
        
        return await _categories.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken cancellationToken = default)
    {
        await _categories.InsertOneAsync(category, cancellationToken: cancellationToken);
        return category;
    }

    public async Task<Category?> UpdateAsync(Category category, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.Id, category.Id);
        var update = Builders<Category>.Update
                .Set(x => x.Name, category.Name)
                .Set(x=>x.Descriprion, category.Descriprion)
                .Set(x=> x.IsActive, category.IsActive)
                .Set(x=>x.ModifiedAt, DateTime.Now);
        var options = new FindOneAndUpdateOptions<Category>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false
        };
        return await _categories.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string categoryId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Category>.Filter.Eq(x => x.Id, categoryId);
        var update = Builders<Category>.Update
            .Set(c=>c.IsDeleted, true)
            .Set(c=>c.ModifiedAt, DateTime.UtcNow);
        var result =  await _categories.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}