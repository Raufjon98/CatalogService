using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.FoodCategory.Responses;
using MediatR;

namespace CatalogService.Api.Features.FoodCategories.Queries;

public record GetFoodCategoriesQuery() : IRequest<List<FoodCategoryResponse>>;

public class GetFoodCategoriesQueryHandler : IRequestHandler<GetFoodCategoriesQuery, List<FoodCategoryResponse>>
{
    private readonly IFoodCategoryRepository  _foodCategoryRepository;

    public GetFoodCategoriesQueryHandler(IFoodCategoryRepository foodCategoryRepository)
    {
        _foodCategoryRepository = foodCategoryRepository;
    }
    public async Task<List<FoodCategoryResponse>> Handle(GetFoodCategoriesQuery request, CancellationToken cancellationToken)
    {
        var foodCategories = await _foodCategoryRepository.GetAllAsync(cancellationToken);
        List<FoodCategoryResponse> result = new List<FoodCategoryResponse>();
        foreach (var foodCategory in foodCategories)
        {
            FoodCategoryResponse foodCategoryResponse = new FoodCategoryResponse()
            {
                Id = foodCategory.Id,
                Name = foodCategory.Name,
                Availability = foodCategory.Availability
            };
            result.Add(foodCategoryResponse);
        };
        return result;
    }
}