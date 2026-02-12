using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Events;
using CatalogService.Contracts.Restaurant.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record UpdateRestaurantCategoryCommand(string RestaurantId, string CategoryId) : IRequest<RestaurantResponse>;
public class UpdateRestaurantCategoryCommandHandler : IRequestHandler<UpdateRestaurantCategoryCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateRestaurantCategoryCommandHandler(IRestaurantRepository restaurantRepository, IPublishEndpoint publishEndpoint)
    {
        _restaurantRepository = restaurantRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<RestaurantResponse> Handle(UpdateRestaurantCategoryCommand request, CancellationToken cancellationToken)
    {
        await _restaurantRepository.UpdateCategoryAsync(request.RestaurantId, request.CategoryId, cancellationToken);
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