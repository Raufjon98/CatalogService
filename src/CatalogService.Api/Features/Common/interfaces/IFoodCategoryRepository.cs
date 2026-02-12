using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface IFoodCategoryRepository
{
    Task<List<FoodCategory>> GetAllAsync(CancellationToken cancellationToken);
    Task<FoodCategory?> GetAsync(string id, CancellationToken cancellationToken);
    Task<FoodCategory> CreateAsync(FoodCategory foodCategory, CancellationToken cancellationToken);
    Task<FoodCategory?> UpdateAsync(FoodCategory foodCategory, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(string id, CancellationToken cancellationToken);
}