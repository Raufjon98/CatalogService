using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Requests;
using CatalogService.Contracts.Category.Responses;
using MediatR;

namespace CatalogService.Api.Features.Categories.Commands;

public record CreateCategoryCommand(CreateCategoryRequest CreateCategoryDto) : IRequest<CategoryResponse>;

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
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
        CategoryResponse categoryResponse = new CategoryResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Descriprion = result.Descriprion
        };
        return categoryResponse;
    }
}