using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Events;
using CatalogService.Contracts.Category.Requests;
using CatalogService.Contracts.Category.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Categories.Commands;

public record CreateCategoryCommand(CreateCategoryRequest CreateCategoryDto) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IPublishEndpoint publishEndpoint)
    {
        _categoryRepository = categoryRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        Category category = new Category()
        {
            Name = request.CreateCategoryDto.Name,
            Descriprion = request.CreateCategoryDto.Descriprion
        };
        var result = await _categoryRepository.CreateAsync(category, cancellationToken);
        if (result is null)
        {
            throw new Exception("Failed to create category");
        }
        
        await _publishEndpoint.Publish(
            new CategoryCreatedEvent
            {
                Id = result.Id,
                CreatedOnUtc = DateTime.UtcNow
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