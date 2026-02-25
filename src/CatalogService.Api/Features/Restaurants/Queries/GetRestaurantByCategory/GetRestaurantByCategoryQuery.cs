using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record GetRestaurantByCategoryQuery(string CategoryId) : IRequest<List<RestaurantResponse>>;
public class GetRestaurantByCategoryQueryHandler : IRequestHandler<GetRestaurantByCategoryQuery, List<RestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantByCategoryQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<List<RestaurantResponse>> Handle(GetRestaurantByCategoryQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetByCategoryAsync(request.CategoryId, cancellationToken);
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