using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Events;
using CatalogService.Contracts.Restaurant.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record UpdateRestaurantAvailabilityCommand(string RestaurantId, bool IsActive) : IRequest<RestaurantResponse>;
public class UpdateRestaurantAvailabilityCommandHandler : IRequestHandler<UpdateRestaurantAvailabilityCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateRestaurantAvailabilityCommandHandler(IRestaurantRepository restaurantRepository, IPublishEndpoint publishEndpoint)
    {
        _restaurantRepository = restaurantRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<RestaurantResponse> Handle(UpdateRestaurantAvailabilityCommand request, CancellationToken cancellationToken)
    {
        await _restaurantRepository.UpdateAvailabilityAsync(request.RestaurantId, request.IsActive, cancellationToken);
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId);
        }

        await _publishEndpoint.Publish(
            new RestaurantUpdatedEvent
            {
                Id = request.RestaurantId,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
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
        return restaurantResponse;
    }
}