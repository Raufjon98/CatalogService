using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Responses;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Queries;

public record GetCuisinesQuery() : IRequest<List<CuisineResponse>>;

public class GetCuisinesQueryHandler : IRequestHandler<GetCuisinesQuery, List<CuisineResponse>>
{
    private readonly ICuisineRepository _cuisineRepository;

    public GetCuisinesQueryHandler(ICuisineRepository cuisineRepository)
    {
        _cuisineRepository = cuisineRepository;
    }
    public async Task<List<CuisineResponse>> Handle(GetCuisinesQuery request, CancellationToken cancellationToken)
    {
        var cuisines = await _cuisineRepository.GetAllAsync(cancellationToken); 
        List<CuisineResponse> result = new List<CuisineResponse>();
        
        foreach (var cuisine in cuisines)
        {
            CuisineResponse cuisineDto = new CuisineResponse()
            {
                Id = cuisine.Id,
                Name = cuisine.Name,
                Description = cuisine.Description,
            };
            result.Add(cuisineDto);
        }
        return result;
    }
}