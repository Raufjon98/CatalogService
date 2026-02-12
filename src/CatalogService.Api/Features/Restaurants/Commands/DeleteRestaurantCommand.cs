using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record DeleteRestaurantCommand(string restaurantId) : IRequest<bool>;
public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand, bool>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public DeleteRestaurantCommandHandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
    {
        var restaurant = await _restaurantRepository.GetAsync(request.restaurantId, cancellationToken);

        if (restaurant is null)
        {
            throw new NotFoundException(nameof(Restaurant), request.restaurantId);
        }
        
        return await _restaurantRepository.DeleteAsync(restaurant, cancellationToken);
    }
}