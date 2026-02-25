using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Category.Responses;
using MediatR;

namespace CatalogService.Api.Features.Categories.Queries;

public record GetCategoriesQuery() : IRequest<List<CategoryResponse>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<CategoryResponse>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoriesQueryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }
    public async Task<List<CategoryResponse>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllAsync(cancellationToken);
        List<CategoryResponse> result = new List<CategoryResponse>();
        foreach (var category in categories)
        {
            CategoryResponse categoryResponse = new CategoryResponse()
            {
                Id = category.Id,
                Name = category.Name,
                Descriprion = category.Descriprion,
            };
            result.Add(categoryResponse);
        }
        return result;
    }
}