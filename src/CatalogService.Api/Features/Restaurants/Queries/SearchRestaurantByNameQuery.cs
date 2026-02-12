using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record SearchRestaurantByNameQuery(string SearchTerm) : IRequest<List<RestaurantResponse>>;
public class SearchRestaurantByNameQueryHandler : IRequestHandler<SearchRestaurantByNameQuery, List<RestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public SearchRestaurantByNameQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<List<RestaurantResponse>> Handle(SearchRestaurantByNameQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.SearchByNameAsync(request.SearchTerm, cancellationToken);
        List<RestaurantResponse> result =  new List<RestaurantResponse>();

        foreach (var restaurant in restaurants)
        {
            RestaurantResponse restaurantResponse = new RestaurantResponse()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                AddressId = restaurant.AddressId,
                CategoryId = restaurant.CategoryId,
                CuisineId = restaurant.CuisineId,
                IsAvailable = restaurant.Availability,
                Phone = restaurant.Phone,
                Email = restaurant.Email,
            };
            result.Add(restaurantResponse);
        }
        return result;
    }
}