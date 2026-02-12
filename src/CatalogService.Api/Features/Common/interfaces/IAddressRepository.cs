using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface IAddressRepository
{
    Task<List<Address>> GetAllAsync(CancellationToken cancellationToken);
    Task<Address?> GetAsync(string id, CancellationToken cancellationToken);
    Task<Address> CreateAsync(Address address, CancellationToken cancellationToken);
    Task<Address> UpdateAsync(Address address, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);   
}