using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Restaurant.Events;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record DeleteRestaurantCommand(string restaurantId) : IRequest<bool>;

public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, bool>
{
    private readonly IRestaurantRepository _restaurantRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository, IPublishEndpoint publishEndpoint)
    {
        _restaurantRepository = restaurantRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.restaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.restaurantId);
        }

        await _restaurantRepository.DeleteAsync(restaurant, cancellationToken);
        await _publishEndpoint.Publish(
            new RestaurantDeletedEvent
            {
                Id = restaurant.Id,
                DeletedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        return true;
    }
}