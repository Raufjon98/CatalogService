using CatalogService.Api.Features.Common.interfaces;
using MediatR;

namespace CatalogService.Api.Features.Cuisines.Commands;

public record DeleteCuisineCommand(string Id): IRequest<bool>;
public class DeleteCuisineCommandHandler : IRequestHandler<DeleteCuisineCommand, bool>
{
    private readonly ICuisineRepository _cuisineRepository;

    public DeleteCuisineCommandHandler(ICuisineRepository cuisineRepository) 
    {
        _cuisineRepository = cuisineRepository;    
    }
    public async Task<bool> Handle(DeleteCuisineCommand request, CancellationToken cancellationToken)
    {
        return await _cuisineRepository.DeleteAsync(request.Id, cancellationToken);
    }
}