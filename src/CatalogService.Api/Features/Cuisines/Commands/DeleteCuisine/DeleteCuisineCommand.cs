using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Cuisine.Events;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Commands;

public record DeleteCuisineCommand(string Id) : IRequest<bool>;

public class DeleteCuisineCommandHandler : IRequestHandler<DeleteCuisineCommand, bool>
{
    private readonly ICuisineRepository _cuisineRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteCuisineCommandHandler(ICuisineRepository cuisineRepository, IPublishEndpoint publishEndpoint)
    {
        _cuisineRepository = cuisineRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(DeleteCuisineCommand request, CancellationToken cancellationToken)
    {
        await _cuisineRepository.DeleteAsync(request.Id, cancellationToken);
        await _publishEndpoint.Publish(
            new CuisineDeletedEvent
            {
                Id = request.Id,
                DeletedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        return true;
    }
}