using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Requests;
using CatalogService.Contracts.Cuisine.Responses;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Commands;

public record CreateCuisineCommad(CreateCuisineRequest Cuisine) : IRequest<CuisineResponse>;

public class CreateCuisineCommandHandler : IRequestHandler<CreateCuisineCommad, CuisineResponse>
{
    private readonly ICuisineRepository _cuisineRepository;

    public CreateCuisineCommandHandler(ICuisineRepository cuisineRepository)
    {
        _cuisineRepository = cuisineRepository;
    }

    public async Task<CuisineResponse> Handle(CreateCuisineCommad request, CancellationToken cancellationToken)
    {
        Cuisine cuisine = new Cuisine()
        {
            Name = request.Cuisine.Name,
            Description = request.Cuisine.Description,
        };
        var result = await _cuisineRepository.CreateAsync(cuisine, cancellationToken);
        if (result is null)
        {
            throw new Exception("Failed to create cuisine");
        }

        CuisineResponse cuisineResponse = new CuisineResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
        };
        return cuisineResponse;
    }
}