using CatalogService.Api.Features.Foods.Commands;
using CatalogService.Api.Features.Foods.Queries;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using CatalogService.Contracts.Interfaces;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class FoodService : ServiceBase<IFoodService>, IFoodService
{
    private readonly IMediator _mediator;

    public FoodService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UnaryResult<FoodResponse> CreateFoodAsync(CreateFoodRequest food)
    {
        var command = new CreateFoodCommand(food);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<FoodResponse> UpdateFoodAsync(string foodId, CreateFoodRequest food)
    {
        var command = new UpdateFoodCommand(foodId, food);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<FoodResponse> UpdateFoodAvailabilityAsync(string foodId, bool isAvailable)
    {
        var command = new UpdateFoodAvailabilityCommand(foodId, isAvailable);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteFoodAsync(string foodId)
    {
        var command = new DeleteFoodCommand(foodId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<FoodResponse> GetFoodAsync(string foodId)
    {
        var query = new GetFoodQuery(foodId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> GetAllFoodsAsync()
    {
        var query = new GetFoodsQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> GetFoodsByCategoryAsync(string categoryId)
    {
        var query = new GetFoodByCategoryIdQuery(categoryId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> GetFoodsByRestaurantAsync(string restaurantId)
    {
        var query = new GetFoodsByRestaurantIdQuery(restaurantId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> GetFoodsByPriceRangeAsync(decimal min, decimal max)
    {
        var query = new GetFooodsByPriceRangeQuery(min, max);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> IncreaseFoodStockAsync(List<FoodStockRequest> requests)
    {
        var command = new IncreaseFoodStockCommand(requests);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<List<FoodResponse>> DecreaseFoodStockAsync(List<FoodStockRequest> requests)
    {
        var command = new DecreaseFoodStockCommand(requests);
        var result = await _mediator.Send(command);
        return result;
    }
}