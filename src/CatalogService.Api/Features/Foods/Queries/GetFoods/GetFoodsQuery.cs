using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Queries;

public class GetFoodsQuery : IRequest<List<FoodResponse>>;

public class GetFoodsQueryHandler : IRequestHandler<GetFoodsQuery, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public GetFoodsQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<List<FoodResponse>> Handle(GetFoodsQuery request, CancellationToken cancellationToken = default)
    {
        var result = new List<FoodResponse>();
        var foods= await _foodRepository.GetAllAsync();
        
        foreach (var food in foods)
        {
            var foodResponse = new FoodResponse()
            {
                Id = food.Id,
                Name = food.Name,
                FoodCategoryId =  food.FoodCategoryId, 
                RestaurantId = food.RestaurantId,
                Stock = food.Stock,
                Price = food.Price,
                IsAvailable = food.Availability,
            };
            result.Add(foodResponse);
        }
        return result;
    }
}
