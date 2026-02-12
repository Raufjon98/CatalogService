using CatalogService.Contracts.FoodCategory.Requests;
using CatalogService.Contracts.FoodCategory.Responses;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface IFoodCategoryService : IService<IFoodCategoryService>
{
    UnaryResult<FoodCategoryResponse> CreateFoodCategoryAsync(CreateFoodCategoryRequest foodCategory);
    UnaryResult<FoodCategoryResponse> UpdateFoodCategoryAsync(string foodCategoryId, CreateFoodCategoryRequest foodCategory);
    UnaryResult<bool> DeleteFoodCategoryAsync(string foodCategoryId);
    UnaryResult<FoodCategoryResponse> GetFoodCategoryAsync(string foodCategoryId);
    UnaryResult<List<FoodCategoryResponse>> GetFoodCategoriesAsync();
}