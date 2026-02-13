using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Events;
using CatalogService.Contracts.Restaurant.Requests;
using CatalogService.Contracts.Restaurant.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record UpdateRestaurantCommand(string RestaurantId, CreateRestaurantRequest UpdateRestaurant)
    : IRequest<RestaurantResponse>;

public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IPublishEndpoint publishEndpoint)
    {
        _restaurantRepository = restaurantRepository;
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish(
            new RestaurantUpdatedEvent
            {
                Id = restaurant.Id,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);

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