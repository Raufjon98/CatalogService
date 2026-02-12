using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record GetActiveRestaurantsQuery : IRequest<List<RestaurantResponse>>;

public class GetActiveRestaurantsQueryHandler : IRequestHandler<GetActiveRestaurantsQuery, List<RestaurantResponse>>
{
    private readonly IRestaurantRepository  _restaurantRepository;

    public GetActiveRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<List<RestaurantResponse>> Handle(GetActiveRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetActiveRestaurantsAsync(cancellationToken);
        List<RestaurantResponse> result = new List<RestaurantResponse>();
        foreach (var restaurant in restaurants)
        {
            RestaurantResponse restaurantResponse = new RestaurantResponse()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                AddressId = restaurant.AddressId,
                IsAvailable = restaurant.Availability,
                CategoryId = restaurant.CategoryId,
                CuisineId = restaurant.CuisineId,
                Email = restaurant.Email,
                Phone = restaurant.Phone
            };
            result.Add(restaurantResponse);
        }
        return result;
    }
}