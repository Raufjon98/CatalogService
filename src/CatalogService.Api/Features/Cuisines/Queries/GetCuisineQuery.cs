using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Responses;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Queries;

public record GetCuisineQuery(string Id) : IRequest<CuisineResponse>;
public class GetCuisineQueryHandler : IRequestHandler<GetCuisineQuery, CuisineResponse>
{
    private readonly ICuisineRepository _cuisineRepository;

    public GetCuisineQueryHandler(ICuisineRepository cuisineRepository)
    {
        _cuisineRepository = cuisineRepository;
    }
    public async Task<CuisineResponse> Handle(GetCuisineQuery request, CancellationToken cancellationToken)
    {
        var cuisine = await _cuisineRepository.GetAsync(request.Id, cancellationToken);

        if (cuisine is null)
        {
            throw new NotFoundException(nameof(Cuisine), request.Id);
        }
        
        CuisineResponse cuisineDto = new CuisineResponse()
        {
            Id = cuisine.Id,
            Name = cuisine.Name,
            Description = cuisine.Description,
        };
        return cuisineDto;
    }
}