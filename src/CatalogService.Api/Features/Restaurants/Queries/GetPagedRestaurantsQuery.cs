using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record GetPagedRestaurantsQuery(int PageNumber, int PageSize) : IRequest<List<RestaurantResponse>>;

public class GetPagedRestaurantsQueryHandler : IRequestHandler<GetPagedRestaurantsQuery, List<RestaurantResponse>>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetPagedRestaurantsQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<List<RestaurantResponse>> Handle(GetPagedRestaurantsQuery request, CancellationToken cancellationToken)
    {
        var restaurants = await _restaurantRepository.GetPagedAsync(request.PageNumber, request.PageSize, cancellationToken);
        List<RestaurantResponse> result = new();
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