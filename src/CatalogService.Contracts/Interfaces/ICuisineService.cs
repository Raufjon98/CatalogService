using CatalogService.Contracts.Cuisine.Requests;
using CatalogService.Contracts.Cuisine.Responses;
using MagicOnion;

namespace CatalogService.Contracts.Interfaces;

public interface ICuisineService : IService<ICuisineService>
{
    UnaryResult<CuisineResponse> CreateCuisineAsync(CreateCuisineRequest cuisine);
    UnaryResult<CuisineResponse> UpdateCuisineAsync(string cuisineId, CreateCuisineRequest cuisine);
    UnaryResult<bool> DeleteCuisineAsync(string cuisineId);
    UnaryResult<CuisineResponse> GetCuisineAsync(string cuisineId);
    UnaryResult<List<CuisineResponse>> GetCuisinesAsync();
}