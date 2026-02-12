using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Queries;

public record GetFoodByCategoryIdQuery(string CategoryId) : IRequest<List<FoodResponse>>;
public class GetFoodByCategoryIdQueryHandler : IRequestHandler<GetFoodByCategoryIdQuery, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public GetFoodByCategoryIdQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<List<FoodResponse>> Handle(GetFoodByCategoryIdQuery request, CancellationToken cancellationToken)
    {
        var foods = await _foodRepository.GetByCategoryIdAsync(request.CategoryId, cancellationToken);
        var result = new List<FoodResponse>();
        foreach (var food in foods)
        {
            FoodResponse foodResponse = new FoodResponse()
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                IsAvailable = food.Availability,
                FoodCategoryId = food.FoodCategoryId,
                RestaurantId = food.RestaurantId,
                Stock = food.Stock
            };
            result.Add(foodResponse);
        }
        return result;
    }
}