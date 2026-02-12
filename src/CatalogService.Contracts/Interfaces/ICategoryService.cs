using CatalogService.Contracts.Category.Requests;
using CatalogService.Contracts.Category.Responses;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface ICategoryService : IService<ICategoryService>
{
    UnaryResult<CategoryResponse> CreateCategoryAsync(CreateCategoryRequest category);
    UnaryResult<CategoryResponse> UpdateCategoryAsync(string categoryId, CreateCategoryRequest category);
    UnaryResult<bool> DeleteCategoryAsync(string categoryId);
    UnaryResult<CategoryResponse> GetCategoryAsync(string categoryId);
    UnaryResult<List<CategoryResponse>> GetAllCategoriesAsync();
}