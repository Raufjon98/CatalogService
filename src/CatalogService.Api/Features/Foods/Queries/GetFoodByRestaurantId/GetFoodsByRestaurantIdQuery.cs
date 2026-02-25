using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Queries;

public record GetFoodsByRestaurantIdQuery(string RestaurantId) : IRequest<List<FoodResponse>>;
public class GetFoodsByRestaurantIdQueryHandler : IRequestHandler<GetFoodsByRestaurantIdQuery, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public GetFoodsByRestaurantIdQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<List<FoodResponse>> Handle(GetFoodsByRestaurantIdQuery request, CancellationToken cancellationToken)
    {
        var foods = await _foodRepository.GetByRestaurantIdAsync(request.RestaurantId, cancellationToken);
        List<FoodResponse> result = new List<FoodResponse>();
        foreach (var food in foods)
        {
            FoodResponse foodResponse = new FoodResponse()
            {
                Id = food.Id,
                Name = food.Name,
                Price = food.Price,
                FoodCategoryId = food.FoodCategoryId,
                RestaurantId = food.RestaurantId,
                Stock = food.Stock,
                IsAvailable = food.Availability,
            };
            result.Add(foodResponse);
        }
        return result;
    }
}