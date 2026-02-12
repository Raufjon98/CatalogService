using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface ICuisineRepository
{
    Task<List<Cuisine>> GetAllAsync(CancellationToken cancellationToken);  
    Task<Cuisine?> GetAsync(string id, CancellationToken cancellationToken);
    Task<Cuisine> CreateAsync(Cuisine cuisine, CancellationToken cancellationToken);
    Task<Cuisine?> UpdateAsync(Cuisine cuisine, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}