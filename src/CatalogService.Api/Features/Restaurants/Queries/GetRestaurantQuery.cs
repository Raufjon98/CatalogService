using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Queries;

public record GetRestaurantQuery(string restaurantId) : IRequest<RestaurantResponse>;

public class GetRestaurantQueryHandler : IRequestHandler<GetRestaurantQuery, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public GetRestaurantQueryHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<RestaurantResponse> Handle(GetRestaurantQuery request, CancellationToken cancellationToken)
    {
        var result = await _restaurantRepository.GetAsync(request.restaurantId, cancellationToken);

        if (result is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.restaurantId);
        }
        
        RestaurantResponse restaurantResponse = new RestaurantResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Description = result.Description,
            CategoryId = result.CategoryId,
            CuisineId = result.CuisineId,
            AddressId = result.AddressId,
            Phone = result.Phone,
            Email = result.Email,
            IsAvailable = result.Availability,
        };
        return restaurantResponse;
    }
}