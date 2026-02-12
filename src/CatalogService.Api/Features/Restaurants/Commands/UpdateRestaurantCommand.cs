using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Requests;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record UpdateRestaurantCommand(string RestaurantId, CreateRestaurantRequest UpdateRestaurant)
    : IRequest<RestaurantResponse>;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<RestaurantResponse> Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId, cancellationToken);
        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId);
        }
        restaurant.Name = request.UpdateRestaurant.Name;
        restaurant.Description = request.UpdateRestaurant.Description;
        restaurant.CategoryId = request.UpdateRestaurant.CategoryId;
        restaurant.CuisineId = request.UpdateRestaurant.CuisineId;
        restaurant.AddressId = request.UpdateRestaurant.AddressId;
        restaurant.Phone = request.UpdateRestaurant.Phone;
        restaurant.Email = request.UpdateRestaurant.Email;
        restaurant.ModifiedAt = DateTime.UtcNow;

        var result = await _restaurantRepository.UpdateAsync(restaurant, cancellationToken);
        if (result is null)
        {
            throw new Exception("Couldn't update restaurant.");
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