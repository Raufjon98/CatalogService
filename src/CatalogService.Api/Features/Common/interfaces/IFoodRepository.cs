using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface IFoodRepository
{
    Task<Food> CreateAsync(Food food, CancellationToken cancellationToken);
    Task<Food> UpdateAsync(Food food, CancellationToken cancellationToken);
    Task<Food> UpdateAvailabilityAsync(string id, bool isAvailable, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Food food, CancellationToken cancellationToken);
    Task<List<Food>> GetAllAsync(string[]? ids = null, CancellationToken cancellationToken = default);
    Task<Food?> GetAsync(string id, CancellationToken cancellationToken);
    Task<List<Food>> GetByCategoryIdAsync(string categoryId, CancellationToken cancellationToken);
    Task<List<Food>> GetByRestaurantIdAsync(string restaurantId, CancellationToken cancellationToken);
    Task<List<Food>> GetByPriceRangeAsync(decimal minPrice, decimal maxPrice, CancellationToken cancellationToken);
}