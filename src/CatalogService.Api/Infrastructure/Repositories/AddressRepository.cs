using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using MongoDB.Driver;

namespace CatalogService.Api.Infrastructure.Repositories;

public class AddressRepository : IAddressRepository
{
    private readonly IMongoCollection<Address> _addressCollection;

    public AddressRepository(MongoDbService mongoDbService)
    {
        if (mongoDbService.Database is null)
            throw new ArgumentNullException(nameof(mongoDbService.Database));
        _addressCollection = mongoDbService.Database.GetCollection<Address>("Addresses");
    }

    public async Task<List<Address>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _addressCollection.Find(Builders<Address>.Filter.Eq(a => a.IsDeleted, false))
            .ToListAsync(cancellationToken);
    }

    public async Task<Address?> GetAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Address>.Filter.And(Builders<Address>.Filter.Eq(x => x.Id, id),
            Builders<Address>.Filter.Eq(x => x.IsDeleted, false));
        return await _addressCollection.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Address> CreateAsync(Address address, CancellationToken cancellationToken = default)
    {
        await _addressCollection.InsertOneAsync(address, cancellationToken: cancellationToken);
        return address;
    }

    public async Task<Address> UpdateAsync(Address address, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Address>.Filter.Eq(x => x.Id, address.Id);

        var update = Builders<Address>.Update
            .Set(x => x.City, address.City)
            .Set(x => x.State, address.State)
            .Set(x => x.ZipCode, address.ZipCode)
            .Set(x => x.Street, address.Street)
            .Set(x => x.House, address.House)
            .Set(x => x.Description, address.Description)
            .Set(x => x.IsDeleted, address.IsDeleted)
            .Set(x => x.ModifiedAt, address.ModifiedAt);

        var options = new FindOneAndUpdateOptions<Address>
        {
            ReturnDocument = ReturnDocument.After,
            IsUpsert = false
        };
        return await _addressCollection.FindOneAndUpdateAsync(filter, update, options, cancellationToken);
    }

    public async Task<bool> DeleteAsync(string id, CancellationToken cancellationToken = default)
    {
        var filter = Builders<Address>.Filter.And(Builders<Address>.Filter.Eq(x => x.Id, id),
            Builders<Address>.Filter.Eq(x => x.IsDeleted, false));
        var update = Builders<Address>.Update
            .Set(x => x.IsDeleted, true)
            .Set(x => x.ModifiedAt, DateTime.UtcNow);
        var result = await _addressCollection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
        return result.ModifiedCount > 0;
    }
}