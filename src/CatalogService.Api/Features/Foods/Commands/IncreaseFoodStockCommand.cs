using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record IncreaseFoodStockCommand(List<FoodStockRequest> Requests) : IRequest<List<FoodResponse>>;

public class IncreaseFoodStockCommandHandler : IRequestHandler<IncreaseFoodStockCommand, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public IncreaseFoodStockCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<List<FoodResponse>> Handle(IncreaseFoodStockCommand command, CancellationToken cancellationToken)
    {
        var requestedFoods = command.Requests.ToDictionary(f => f.FoodId, f => f.Quantity);
        var ids = requestedFoods.Keys.ToArray();
        var foods = await _foodRepository.GetAllAsync(ids, cancellationToken);
        var foodResponses = new List<FoodResponse>();
        foreach (var food in foods)
        {
            if (!requestedFoods.TryGetValue(food.Id, out var quantity))
                continue;

            food.Stock += quantity;
            var result = await _foodRepository.UpdateAsync(food, cancellationToken);
            foodResponses.Add(new FoodResponse
            {
                Id = result.Id,
                Name = result.Name,
                Stock = result.Stock,
                FoodCategoryId = result.FoodCategoryId,
                RestaurantId = result.RestaurantId,
                Price = result.Price,
                IsAvailable = result.Availability,
            });
        }

        return foodResponses;
    }
}