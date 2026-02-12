using CatalogService.Api.Domain.Entities;

namespace CatalogService.Api.Features.Common.interfaces;

public interface IRestaurantRepository
{
    Task<Restaurant> CreateAsync(Restaurant restaurant, CancellationToken cancellationToken);
    Task<Restaurant> UpdateAsync(Restaurant restaurant, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Restaurant restaurant, CancellationToken cancellationToken);
    Task<List<Restaurant>> GetAllAsync(CancellationToken cancellationToken);
    Task<Restaurant?> GetAsync(string id, CancellationToken cancellationToken);
    Task<List<Restaurant>> GetPagedAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<Restaurant> UpdateAvailabilityAsync(string id, bool isAvailable, CancellationToken cancellationToken);
    Task<bool> UpdateCategoryAsync(string id, string categoryId, CancellationToken cancellationToken);
    Task<bool> DeleteInactiveAsync(CancellationToken cancellationToken);
    Task<List<Restaurant>> SearchByNameAsync(string searchTerm, CancellationToken cancellationToken);
    Task<List<Restaurant>> GetActiveRestaurantsAsync(CancellationToken cancellationToken);
    Task<List<Restaurant>> GetAvailableRestaurantsAsync(CancellationToken cancellationToken);
    Task<List<Restaurant>> GetByCuisineAsync(string cuisineId, CancellationToken cancellationToken);
    Task<List<Restaurant>> GetByCategoryAsync(string categoryId, CancellationToken cancellationToken);
}