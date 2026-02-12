using CatalogService.Api.Features.FoodCategories.Commands;
using CatalogService.Api.Features.FoodCategories.Queries;
using CatalogService.Contracts.FoodCategory.Requests;
using CatalogService.Contracts.FoodCategory.Responses;
using CatalogService.Contracts.Interfaces;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class FoodCategoryService : ServiceBase<IFoodCategoryService>, IFoodCategoryService
{
    private readonly IMediator _mediator;

    public FoodCategoryService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UnaryResult<FoodCategoryResponse> CreateFoodCategoryAsync(CreateFoodCategoryRequest foodCategory)
    {
        var command = new CreateFoodCategoryCommand(foodCategory);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<FoodCategoryResponse> UpdateFoodCategoryAsync(string foodCategoryId, CreateFoodCategoryRequest foodCategory)
    {
        var command = new UpdateFoodCategoryCommand(foodCategoryId, foodCategory);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteFoodCategoryAsync(string foodCategoryId)
    {
        var command = new DeleteFoodCategoryCommand(foodCategoryId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<FoodCategoryResponse> GetFoodCategoryAsync(string foodCategoryId)
    {
        var query = new GetFoodCategoryQuery(foodCategoryId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<FoodCategoryResponse>> GetFoodCategoriesAsync()
    {
        var query = new GetFoodCategoriesQuery();
        var result = await _mediator.Send(query);
        return result;
    }
}