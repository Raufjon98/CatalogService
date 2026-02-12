using CatalogService.Api.Features.Restaurants.Commands;
using CatalogService.Api.Features.Restaurants.Queries;
using CatalogService.Contracts.Interfaces;
using CatalogService.Contracts.Restaurant.Requests;
using CatalogService.Contracts.Restaurant.Responses;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class RestaurantService : ServiceBase<IRestaurantService>, IRestaurantService
{
    private readonly IMediator _mediator;

    public RestaurantService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UnaryResult<RestaurantResponse> CreateRestaurantAsync(CreateRestaurantRequest restaurant)
    {
        var command = new CreateRestaurantCommand(restaurant);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<RestaurantResponse> UpdateRestaurantAsync(string restaurantId, CreateRestaurantRequest restaurant)
    {
        var command = new UpdateRestaurantCommand(restaurantId, restaurant);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<RestaurantResponse> UpdateRestauranAvailabilitytAsync(string restaurantId, bool isAvailable)
    {
        var command = new UpdateRestaurantAvailabilityCommand(restaurantId, isAvailable);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<RestaurantResponse> UpdateRestaurantCategoryAsync(string restaurantId, string categoryId)
    {
        var command = new UpdateRestaurantCategoryCommand(restaurantId, categoryId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteInActiveRestaurantAsync()
    {
        var command = new DeleteInActiveRestaurantCommand();
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteRestaurantAsync(string restaurantId)
    {
        var command = new DeleteRestaurantCommand(restaurantId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetAllRestaurantsAsync()
    {
        var query = new GetRestaurantsQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<RestaurantResponse> GetRestaurantAsync(string restaurantId)
    {
        var query = new GetRestaurantQuery(restaurantId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> SearchRestaurantByNameAsync(string searchTerm)
    {
        var query = new SearchRestaurantByNameQuery(searchTerm);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetPagedRestaurantsAsync(int page, int pageSize)
    {
        var query = new GetPagedRestaurantsQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetRestaurantByCuisineAsync(string cuisineId)
    {
        var query = new GetRestaurantByCuisineQuery(cuisineId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetRestaurantsByCategoryAsync(string categoryId)
    {
        var query = new GetRestaurantByCategoryQuery(categoryId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetAvailableRestaurantsAsync()
    {
        var query = new GetAvailableRestaurantQuery();
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<RestaurantResponse>> GetActiveRestaurantsAsync()
    {
        var query = new GetActiveRestaurantsQuery();
        var result = await _mediator.Send(query);
        return result;
    }
}