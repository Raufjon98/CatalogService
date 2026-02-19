using CatalogService.Api.MagicOnion.Services;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using CatalogService.Contracts.Interfaces;

namespace CatalogService.Api.Tests.Integration.Endoints.Models;

public class FoodTestScenarioBuilder
{
    private readonly IFoodService _foodService;


    public FoodTestScenarioBuilder(
        IFoodService foodService)
    {
        _foodService = foodService;
    }

    public async Task<FoodResponse> CreateFoodAsync()
    {
        var createFoodRequest = new CreateFoodRequest
        {
            Name = "Soup",
            FoodCategoryId = Guid.NewGuid().ToString(),
            restaurantId = Guid.NewGuid().ToString(),
            Stock = 50,
            Price = 16
        };
        
        var response = await _foodService.CreateFoodAsync(createFoodRequest);
        return response;
    }
}