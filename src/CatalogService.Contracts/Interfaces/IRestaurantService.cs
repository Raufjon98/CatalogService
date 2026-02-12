using CatalogService.Contracts.Restaurant.Requests;
using CatalogService.Contracts.Restaurant.Responses;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface IRestaurantService : IService<IRestaurantService>
{
    UnaryResult<RestaurantResponse> CreateRestaurantAsync(CreateRestaurantRequest restaurant);
    UnaryResult<RestaurantResponse> UpdateRestaurantAsync(string restaurantId, CreateRestaurantRequest restaurant);
    UnaryResult<RestaurantResponse> UpdateRestauranAvailabilitytAsync(string restaurantId, bool isAvailable);
    UnaryResult<RestaurantResponse> UpdateRestaurantCategoryAsync(string restaurantId, string categoryId);
    UnaryResult<bool> DeleteInActiveRestaurantAsync();
    UnaryResult<bool> DeleteRestaurantAsync(string restaurantId);
    UnaryResult<List<RestaurantResponse>> GetAllRestaurantsAsync();
    UnaryResult<RestaurantResponse> GetRestaurantAsync(string restaurantId);
    UnaryResult<List<RestaurantResponse>> SearchRestaurantByNameAsync(string searchTerm);
    UnaryResult<List<RestaurantResponse>> GetPagedRestaurantsAsync(int page, int pageSize);
    UnaryResult<List<RestaurantResponse>> GetRestaurantByCuisineAsync(string cuisineId);
    UnaryResult<List<RestaurantResponse>> GetRestaurantsByCategoryAsync(string categoryId);
    UnaryResult<List<RestaurantResponse>> GetAvailableRestaurantsAsync();
    UnaryResult<List<RestaurantResponse>> GetActiveRestaurantsAsync();
}