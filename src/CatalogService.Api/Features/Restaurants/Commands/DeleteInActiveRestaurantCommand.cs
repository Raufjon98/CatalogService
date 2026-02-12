using CatalogService.Api.Features.Common.interfaces;
using MediatR;

namespace CatalogService.Api.Features.Restaurants.Commands;

public record DeleteInActiveRestaurantCommand : IRequest<bool>;

public class DeleteInActiveRestaurantCommandhandler : IRequestHandler<DeleteInActiveRestaurantCommand, bool>
{
    private readonly IRestaurantRepository _restaurantRepository;

    public DeleteInActiveRestaurantCommandhandler(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }
    public async Task<bool> Handle(DeleteInActiveRestaurantCommand request, CancellationToken cancellationToken)
    {
        return await _restaurantRepository.DeleteInactiveAsync(cancellationToken);
    }
}