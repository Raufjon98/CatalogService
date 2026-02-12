using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface IFoodService : IService<IFoodService>
{
    UnaryResult<FoodResponse> CreateFoodAsync(CreateFoodRequest food);
    UnaryResult<FoodResponse> UpdateFoodAsync(string foodId, CreateFoodRequest food);
    UnaryResult<FoodResponse> UpdateFoodAvailabilityAsync(string foodId, bool isAvailable);
    UnaryResult<bool> DeleteFoodAsync(string foodId);
    UnaryResult<FoodResponse> GetFoodAsync(string foodId);
    UnaryResult<List<FoodResponse>> GetAllFoodsAsync();
    UnaryResult<List<FoodResponse>> GetFoodsByCategoryAsync(string categoryId);
    UnaryResult<List<FoodResponse>> GetFoodsByRestaurantAsync(string restaurantId);
    UnaryResult<List<FoodResponse>> GetFoodsByPriceRangeAsync(decimal min, decimal max);
    UnaryResult<List<FoodResponse>> IncreaseFoodStockAsync(List<FoodStockRequest> requests);
    UnaryResult<List<FoodResponse>> DecreaseFoodStockAsync(List<FoodStockRequest> requests);
}