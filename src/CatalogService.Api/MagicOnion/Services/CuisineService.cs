using CatalogService.Api.Features.Cuisines.Commands;
using CatalogService.Api.Features.Cuisines.Queries;
using CatalogService.Contracts.Cuisine.Requests;
using CatalogService.Contracts.Cuisine.Responses;
using CatalogService.Contracts.Interfaces;
using MagicOnion;
using MagicOnion.Server;
using MediatR;

namespace CatalogService.Api.MagicOnion.Services;

public class CuisineService : ServiceBase<ICuisineService>, ICuisineService
{
    private readonly IMediator _mediator;

    public CuisineService(IMediator mediator)
    {
        _mediator = mediator;
    }
    public async UnaryResult<CuisineResponse> CreateCuisineAsync(CreateCuisineRequest cuisine)
    {
        var command = new CreateCuisineCommad(cuisine);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<CuisineResponse> UpdateCuisineAsync(string cuisineId, CreateCuisineRequest cuisine)
    {
        var command = new UpdateCuisineCommand(cuisineId, cuisine);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<bool> DeleteCuisineAsync(string cuisineId)
    {
        var command = new DeleteCuisineCommand(cuisineId);
        var result = await _mediator.Send(command);
        return result;
    }

    public async UnaryResult<CuisineResponse> GetCuisineAsync(string cuisineId)
    {
        var query = new GetCuisineQuery(cuisineId);
        var result = await _mediator.Send(query);
        return result;
    }

    public async UnaryResult<List<CuisineResponse>> GetCuisinesAsync()
    {
        var query = new GetCuisinesQuery();
        var result = await _mediator.Send(query);
        return result;
    }
}