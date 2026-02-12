using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Queries;

public record GetFooodsByPriceRangeQuery(decimal MinValue, decimal MaxValue) : IRequest<List<FoodResponse>>;
public class GetFooodsByPriceRangeQueryHandler : IRequestHandler<GetFooodsByPriceRangeQuery, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public GetFooodsByPriceRangeQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<List<FoodResponse>> Handle(GetFooodsByPriceRangeQuery request, CancellationToken cancellationToken)
    {
        var foods = await _foodRepository.GetByPriceRangeAsync(request.MinValue, request.MaxValue, cancellationToken);
        List<FoodResponse> result = new List<FoodResponse>();
        foreach (var food in foods)
        {
            FoodResponse foodResponse = new FoodResponse()
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                IsAvailable = food.Availability,
                RestaurantId = food.RestaurantId,
                FoodCategoryId = food.FoodCategoryId,
                Stock = food.Stock
            };
            result.Add(foodResponse);
        }
        return result;
    }
}