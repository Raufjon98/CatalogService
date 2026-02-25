using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Events;
using CatalogService.Contracts.Category.Requests;
using CatalogService.Contracts.Category.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Categories.Commands;

public record UpdateCategoryCommand(string Id, CreateCategoryRequest UpdateCategory) : IRequest<CategoryResponse>;

public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IPublishEndpoint publishEndpoint)
    {
        _categoryRepository = categoryRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CategoryResponse> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = new Category()
        {
            Id = request.Id,
            Name = request.UpdateCategory.Name,
            Descriprion = request.UpdateCategory.Descriprion
        };
        var result = await _categoryRepository.UpdateAsync(category, cancellationToken);

        if (result is null)
        {
            throw new Exception("Could not update category");
        }

        await _publishEndpoint.Publish(
            new CategoryUpdatedEvent
            {
                Id = request.Id,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);

        CategoryResponse categoryResponse = new CategoryResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Descriprion = result.Descriprion
        };
        return categoryResponse;
    }
}