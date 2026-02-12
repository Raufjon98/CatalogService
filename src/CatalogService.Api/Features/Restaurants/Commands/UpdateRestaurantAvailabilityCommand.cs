using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Responses;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record UpdateRestaurantAvailabilityCommand(string RestaurantId, bool IsActive) : IRequest<RestaurantResponse>;
public class UpdateRestaurantAvailabilityCommandHandler : IRequestHandler<UpdateRestaurantAvailabilityCommand, RestaurantResponse>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public UpdateRestaurantAvailabilityCommandHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<RestaurantResponse> Handle(UpdateRestaurantAvailabilityCommand request, CancellationToken cancellationToken)
    {
        await _restaurantRepository.UpdateAvailabilityAsync(request.RestaurantId, request.IsActive, cancellationToken);
        var restaurant = await _restaurantRepository.GetAsync(request.RestaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.RestaurantId);
        }
        
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