using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record GetRestaurantsQuery : IRequest<List<RestaurantResponse>>;
public class GetRestaurantsQueryHandler : IRequestHandler<GetRestaurantsQuery, List<RestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<List<RestaurantResponse>> Handle(GetRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetAllAsync(cancellationToken);
        List<RestaurantResponse> result = new List<RestaurantResponse>();
        foreach (var restaurant in restaurants)
        {
            RestaurantResponse restaurantResponse = new RestaurantResponse()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Description = restaurant.Description,
                CategoryId = restaurant.CategoryId,
                CuisineId = restaurant.CuisineId,
                AddressId = restaurant.AddressId,
                Phone = restaurant.Phone,
                Email = restaurant.Email,
                IsAvailable = restaurant.Availability,
            };
            result.Add(restaurantResponse);
        }
        return result;
    }
}