using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Responses;
using MediatR;

namespace CatalogService.Api.Features.Categories.Queries;

public record GetCategoryQuery(string CategoryId) : IRequest<CategoryResponse>;
public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryQueryHandler(ICategoryRepository categoryRepository) 
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<CategoryResponse> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            throw new NotFoundException(nameof(Category), request.CategoryId);
        }
        
        CategoryResponse result = new CategoryResponse()
        {
            Id = category.Id,
            Name = category.Name,
            Descriprion = category.Descriprion,
        };
        return result;
    }
}