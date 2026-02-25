using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Events;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Categories.Commands;

public record DeleteCategoryCommand(string Id) : IRequest<bool>;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IPublishEndpoint publishEndpoint)
    {
        _categoryRepository = categoryRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteAsync(request.Id, cancellationToken);
        
        await _publishEndpoint.Publish(
            new CategoryDeletedEvent
            {
                Id = request.Id,
                DeletedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        return true;
    }
}